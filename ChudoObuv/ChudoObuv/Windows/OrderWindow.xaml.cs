using ChudoObuv.DbContexts;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace ChudoObuv.Windows;

public partial class OrderWindow : Window
{
    private readonly bool management;

    private Order? SelectedOrder
    {
        get { return ordersGrid.SelectedItem as Order; }
    }

    public OrderWindow(bool management = false)
    {
        InitializeComponent();
        this.management = management;
        userTb.Text = CurrentUser.FullName;

        if (!CurrentUser.IsAuthenticated || (management && !CurrentUser.CanManageOrders))
        {
            Loaded += CloseWindow;
            return;
        }

        ConfigureWindow();
        try
        {
            if (management)
            {
                LoadOrders();
            }
            else
            {
                LoadClients();
            }
        }
        catch (Exception ex)
        {
            Error(ex);
        }

        if (!management)
        {
            RefreshCart();
        }
    }

    private void ConfigureWindow()
    {
        if (management)
        {
            Title = "Чудо Обувь — Управление заказами";
            cartPanel.Visibility = Visibility.Collapsed;
            cartGrid.Visibility = Visibility.Collapsed;
            cartActions.Visibility = Visibility.Collapsed;
            confirmBtn.Visibility = Visibility.Collapsed;
            ordersGrid.Visibility = Visibility.Visible;
            itemsGrid.Visibility = Visibility.Visible;
            manageActions.Visibility = Visibility.Visible;
        }
        else
        {
            Title = "Чудо Обувь — Корзина";
            orderItemsRow.Height = new GridLength(0);
        }

        if (!CurrentUser.IsAdmin)
        {
            orderDatePicker.Visibility = Visibility.Collapsed;
            saveDateBtn.Visibility = Visibility.Collapsed;
            removeItemBtn.Visibility = Visibility.Collapsed;
        }
    }

    private static void Error(Exception ex)
    {
        string message = "Операция не выполнена. Проверьте подключение к БД и актуальность остатков, затем повторите.\n" + ex.Message;
        if (ex is DbUpdateConcurrencyException)
        {
            message = "Данные уже изменены другим пользователем. Откройте окно заново, проверьте остатки и повторите действие.";
        }
        MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void LoadClients()
    {
        using var db = new ChudoobuvContext();
        clientCb.ItemsSource = db.Users.OrderBy(user => user.LastName).ToList();
        clientCb.SelectedValue = CurrentUser.Id;
        clientCb.IsEnabled = CurrentUser.CanManageOrders;
    }

    private void RefreshCart()
    {
        cartGrid.ItemsSource = null;
        cartGrid.ItemsSource = AppData.Cart;
        totalTb.Text = $"Итого: {AppData.Cart.Sum(item => item.Sum):F2} ₽";
        confirmBtn.IsEnabled = AppData.Cart.Count > 0;

        if (AppData.Cart.Count == 0)
        {
            hintTb.Text = "Корзина пуста. Вернитесь в каталог и выберите товары.";
        }
        else
        {
            hintTb.Text = "Цена и наличие повторно проверяются при подтверждении заказа.";
        }
    }

    private void ChangeQuantity(int change)
    {
        CartItem? item = cartGrid.SelectedItem as CartItem;
        if (item == null)
        {
            return;
        }

        int quantity = item.Quantity + change;
        if (quantity <= 0)
        {
            AppData.Cart.Remove(item);
        }
        else if (quantity <= item.MaxQuantity)
        {
            item.Quantity = quantity;
        }
        else
        {
            MessageBox.Show("Достигнут доступный остаток. Выберите другую модель или размер.", "Недостаточно товара", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        RefreshCart();
        cartGrid.SelectedItem = item;
    }

    private void minusBtn_Click(object sender, RoutedEventArgs e)
    {
        ChangeQuantity(-1);
    }

    private void plusBtn_Click(object sender, RoutedEventArgs e)
    {
        ChangeQuantity(1);
    }

    private void removeCartBtn_Click(object sender, RoutedEventArgs e)
    {
        CartItem? item = cartGrid.SelectedItem as CartItem;
        if (item != null)
        {
            AppData.Cart.Remove(item);
        }
        RefreshCart();
    }

    private void clearBtn_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult answer = MessageBox.Show("Очистить корзину? Заказ не будет оформлен.", "Отказ от заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer == MessageBoxResult.Yes)
        {
            AppData.Cart.Clear();
            RefreshCart();
        }
    }

    private void confirmBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!CurrentUser.IsAuthenticated || management || AppData.Cart.Count == 0)
        {
            return;
        }
        if (clientCb.SelectedValue == null)
        {
            MessageBox.Show("Выберите клиента заказа.", "Проверка заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        int clientId = (int)clientCb.SelectedValue;
        if (!CurrentUser.CanManageOrders && clientId != CurrentUser.Id)
        {
            MessageBox.Show("Выберите клиента заказа.", "Проверка заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        confirmBtn.IsEnabled = false;
        try
        {
            Order? order = CreateOrder(clientId);
            if (order == null)
            {
                MessageBox.Show("Цена товаров изменилась. Корзина пересчитана. Проверьте сумму и подтвердите заказ повторно.", "Изменение цены", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AppData.Cart.Clear();
            MessageBox.Show($"Заказ №{order.IdOrder} оформлен. Сумма: {order.Total:F2} ₽.", "Заказ оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            RefreshCart();
        }
    }

    private static Order? CreateOrder(int clientId)
    {
        if (!CurrentUser.IsAuthenticated || AppData.Cart.Count == 0)
        {
            throw new InvalidOperationException("Нет доступа к оформлению заказа.");
        }
        if (!CurrentUser.CanManageOrders && clientId != CurrentUser.Id)
        {
            throw new InvalidOperationException("Нет доступа к оформлению заказа.");
        }

        using var db = new ChudoobuvContext();
        Order order = new Order();
        order.UserId = clientId;
        bool priceChanged = false;

        foreach (CartItem item in AppData.Cart)
        {
            Stockitem? stock = db.Stockitems.Find(item.StockItemId);
            if (stock == null)
            {
                throw new InvalidOperationException("Товарная позиция больше не существует. Удалите её из корзины.");
            }
            item.MaxQuantity = stock.Quantity;
            if (item.Quantity <= 0 || item.Quantity > stock.Quantity)
            {
                throw new InvalidOperationException($"Недостаточно товара: {item.ProdName}, размер {item.SizeName}. Измените количество в корзине.");
            }

            // История заказов нужна для расчёта скидки за прошлый месяц.
            Product? product = db.Products.Include(p => p.Ordersitems)
                .ThenInclude(line => line.Order).FirstOrDefault(p => p.IdProd == stock.ProdId);
            if (product == null)
            {
                throw new InvalidOperationException("Модель товара не найдена. Удалите её из корзины.");
            }
            decimal price = product.FinalPrice;
            if (item.UnitPrice != price)
            {
                item.UnitPrice = price;
                priceChanged = true;
            }

            Ordersitem line = new Ordersitem();
            line.ProdId = stock.ProdId;
            line.SizeId = stock.SizeId;
            line.Quantity = item.Quantity;
            line.UnitPrice = price;
            order.Ordersitems.Add(line);
            stock.Quantity = stock.Quantity - item.Quantity;
        }

        if (priceChanged)
        {
            return null;
        }

        // Заказ, его строки и остатки сохраняются вместе одним вызовом.
        order.OrderDate = DateTime.Now;
        db.Orders.Add(order);
        db.SaveChanges();
        return order;
    }

    private void LoadOrders(int? selectId = null)
    {
        if (!CurrentUser.CanManageOrders)
        {
            return;
        }
        using var db = new ChudoobuvContext();
        List<Order> orders = db.Orders.Include(order => order.User)
            .Include(order => order.Ordersitems).ThenInclude(line => line.Prod).ThenInclude(product => product!.Manuf)
            .Include(order => order.Ordersitems).ThenInclude(line => line.Size)
            .OrderByDescending(order => order.OrderDate).ThenByDescending(order => order.IdOrder).ToList();
        ordersGrid.ItemsSource = orders;

        if (orders.Count == 0)
        {
            itemsGrid.ItemsSource = null;
            totalTb.Text = "Заказов нет";
            hintTb.Text = "";
            return;
        }
        ordersGrid.SelectedIndex = 0;
        foreach (Order order in orders)
        {
            if (order.IdOrder == selectId)
            {
                ordersGrid.SelectedItem = order;
                break;
            }
        }
    }

    private void ordersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Order? order = SelectedOrder;
        itemsGrid.ItemsSource = null;
        orderDatePicker.SelectedDate = null;
        totalTb.Text = "";
        if (order != null)
        {
            itemsGrid.ItemsSource = order.Ordersitems;
            orderDatePicker.SelectedDate = order.OrderDate;
            totalTb.Text = $"Итого: {order.TotalText}";
        }
        hintTb.Text = "При удалении заказа или позиции зарезервированные пары возвращаются в каталог.";
    }

    // itemId не указан — удаляем весь заказ; указан — одну строку.
    private static void DeleteOrder(int orderId, int? itemId = null)
    {
        using var db = new ChudoobuvContext();
        Order? order = db.Orders.Include(o => o.Ordersitems).FirstOrDefault(o => o.IdOrder == orderId);
        if (order == null)
        {
            throw new InvalidOperationException("Заказ уже удалён. Откройте список заказов заново.");
        }

        List<Ordersitem> items = new List<Ordersitem>();
        foreach (Ordersitem item in order.Ordersitems)
        {
            if (itemId == null || item.IdOrderItem == itemId)
            {
                items.Add(item);
            }
        }
        if (itemId != null && items.Count == 0)
        {
            throw new InvalidOperationException("Позиция уже удалена. Откройте список заказов заново.");
        }

        foreach (Ordersitem item in items)
        {
            Stockitem? stock = db.Stockitems.FirstOrDefault(s => s.ProdId == item.ProdId && s.SizeId == item.SizeId);
            if (stock == null)
            {
                throw new InvalidOperationException("Товарная позиция не найдена в остатках. Обратитесь к администратору.");
            }
            stock.Quantity = stock.Quantity + item.Quantity;
        }
        if (items.Count == order.Ordersitems.Count)
        {
            db.Orders.Remove(order);
        }
        else
        {
            db.Ordersitems.RemoveRange(items);
        }
        db.SaveChanges();
    }

    private void deleteBtn_Click(object sender, RoutedEventArgs e)
    {
        Order? order = SelectedOrder;
        if (!CurrentUser.CanManageOrders || order == null)
        {
            return;
        }
        MessageBoxResult answer = MessageBox.Show($"Удалить заказ №{order.IdOrder}? Операцию нельзя отменить. Остатки будут возвращены в каталог.", "Удаление заказа", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (answer != MessageBoxResult.Yes)
        {
            return;
        }
        try
        {
            DeleteOrder(order.IdOrder);
            LoadOrders();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
    }

    private void orderDatePicker_DateValidationError(object? sender, DatePickerDateValidationErrorEventArgs e)
    {
        e.ThrowException = false;
        MessageBox.Show("Введите дату в формате ДД.ММ.ГГГГ или выберите её в календаре.", "Некорректная дата", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private void saveDateBtn_Click(object sender, RoutedEventArgs e)
    {
        Order? selected = SelectedOrder;
        if (!CurrentUser.IsAdmin || selected == null)
        {
            return;
        }
        DateTime? date = orderDatePicker.SelectedDate;
        if (date == null || date.Value.Date > DateTime.Today)
        {
            MessageBox.Show("Выберите корректную дату заказа, не позднее сегодняшней.", "Некорректная дата", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        try
        {
            using var db = new ChudoobuvContext();
            Order order = db.Orders.First(o => o.IdOrder == selected.IdOrder);
            order.OrderDate = date.Value.Date;
            db.SaveChanges();
            LoadOrders(selected.IdOrder);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
    }

    private void removeItemBtn_Click(object sender, RoutedEventArgs e)
    {
        Order? order = SelectedOrder;
        Ordersitem? item = itemsGrid.SelectedItem as Ordersitem;
        if (!CurrentUser.IsAdmin || order == null || item == null)
        {
            return;
        }
        MessageBoxResult answer = MessageBox.Show("Удалить выбранную позицию? Последняя позиция будет удалена вместе с заказом. Остатки вернутся в каталог.", "Удаление позиции", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (answer != MessageBoxResult.Yes)
        {
            return;
        }
        try
        {
            DeleteOrder(order.IdOrder, item.IdOrderItem);
            LoadOrders(order.IdOrder);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
    }

    private void newBtn_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Выберите товары в каталоге, затем откройте корзину и укажите клиента перед подтверждением.", "Новый заказ", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
