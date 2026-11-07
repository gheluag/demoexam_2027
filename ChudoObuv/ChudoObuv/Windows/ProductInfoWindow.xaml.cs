using System.Windows;
using System.Windows.Controls;
using ChudoObuv.DbContexts;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore; 

namespace ChudoObuv.Windows;

public partial class ProductInfoWindow : Window
{
    private Product? product;
    public ProductInfoWindow(int productId)
    {
        InitializeComponent();
        LoadProduct(productId);
        orderPanel.Visibility = CurrentUser.Role == "Гость"
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void LoadProduct(int productId)
    {
        try
        {
            using var db = new ChudoobuvContext();

            product = db.Products
                .Include(p => p.Cat)
                .Include(p => p.Manuf)
                .Include(p => p.Stockitems)
                    .ThenInclude(s => s.Size)
                .Include(p => p.Ordersitems)
                    .ThenInclude(o => o.Order)
                .FirstOrDefault(p => p.IdProd == productId);

            if (product == null)
            {
                MessageBox.Show("Товар не найден", "ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataContext = product;

            sizeRangeTb.Text = string.Join(", ",
                product.Stockitems
                    .Where(s => s.Size != null)
                    .Select(s => s.Size!.Size1)
                    .Distinct()
                    .OrderBy(s => s));

            sizeCb.ItemsSource = product.Stockitems
                .Where(s => s.Quantity > 0)
                .OrderBy(s => s.Size!.Size1)
                .ToList();

            if (sizeCb.Items.Count > 0)
            {
                sizeCb.SelectedIndex = 0;
                addBtn.IsEnabled = true;
            }
            else
            {
                stockTb.Text = "Нет доступных размеров";
                quantityCb.ItemsSource = null;
                addBtn.IsEnabled = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка загрузки:\n" + ex.Message, "ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void addBtn_Click(object sender, RoutedEventArgs e)
    {
        if (product == null || !CurrentUser.IsAuthenticated)
            return;

        if (sizeCb.SelectedItem is not Stockitem stock ||
            quantityCb.SelectedItem is not int quantity)
        {
            MessageBox.Show("Выберите размер и количество", "ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var cartItem = AppData.Cart
            .FirstOrDefault(x => x.StockItemId == stock.IdStockItem);

        int addedQuantity = cartItem?.Quantity ?? 0;

        if (addedQuantity + quantity > stock.Quantity)
        {
            MessageBox.Show("Выбрано больше товара, чем доступно для заказа", "ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            AppData.Cart.Add(new CartItem
            {
                ProdId = product.IdProd,
                StockItemId = stock.IdStockItem,
                ProdName = product.ProdName ?? "",
                ManufName = product.Manuf?.ManufName ?? "",
                SizeName = stock.Size?.Size1 ?? "",
                Quantity = quantity,
                MaxQuantity = stock.Quantity,
                UnitPrice = product.FinalPrice
            });
        }

        MessageBox.Show("Товар добавлен в заказ", "информация",
            MessageBoxButton.OK, MessageBoxImage.Information);

        Close();
    }

    private void backBtn_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void sizeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sizeCb.SelectedItem is not Stockitem stock)
            return;

        int quantity = stock.Quantity;

        var cartItem = AppData.Cart.FirstOrDefault(x => x.StockItemId == stock.IdStockItem);
        int addedQuantity = cartItem?.Quantity ?? 0;

        int available = quantity - addedQuantity;

        stockTb.Text = $"Остаток: {available}";

        quantityCb.ItemsSource = Enumerable.Range(1, Math.Max(0, available));
        addBtn.IsEnabled = available > 0;

        if (available > 0)
            quantityCb.SelectedIndex = 0;
    }
}
