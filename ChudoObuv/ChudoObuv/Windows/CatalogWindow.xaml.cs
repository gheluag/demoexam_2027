using ChudoObuv.DbContexts;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace ChudoObuv.Windows;

public partial class CatalogWindow : Window
{
    public CatalogWindow()
    {
        InitializeComponent();
        try
        {
            LoadCategories();
            LoadProd();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Не удалось загрузить категории.\n" + ex.Message, "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        LoadConfig();
        UserTb.Text = CurrentUser.FullName;
        UpdateCartButton();
    }

    private void LoadConfig()
    {
        ordersBtn.Visibility = CurrentUser.CanManageOrders ? Visibility.Visible : Visibility.Collapsed;
        var visibility = CurrentUser.Role == "Гость" ? Visibility.Collapsed : Visibility.Visible;
        SortGrid.Visibility = visibility;
        orderBtn.Visibility = visibility;
    }

    private void prodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (prodList.SelectedItem is not Product product)
            return;

        prodList.SelectedItem = null;

        var window = new ProductInfoWindow(product.IdProd);
        window.Owner = this;
        window.ShowDialog();

        UpdateCartButton();
    }

    private void orderBtn_Click(object sender, RoutedEventArgs e)
    {
        var window = new OrderWindow();
        window.Owner = this;

        if (window.ShowDialog() == true)
            LoadProd();

        UpdateCartButton();
    }

    private void ordersBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!CurrentUser.CanManageOrders)
            return;
        var window = new OrderWindow(true) { Owner = this };
        window.ShowDialog();
        LoadProd();
        UpdateCartButton();
    }

    private void UpdateCartButton()
    {
        orderBtn.Content = $"Корзина ({AppData.Cart.Sum(x => x.Quantity)})";
    }
    private void exitBtn_Click(object sender, RoutedEventArgs e)
    {
        var mn = new AuthWindow();
        Application.Current.MainWindow = mn;
        mn.Show();
        Close();
    }

    private void LoadCategories()
    {
        using var db = new ChudoobuvContext();

        var categories = db.Categories.OrderBy(c => c.CatName).ToList();

        categories.Insert(0, new Category
        {
            IdCat = 0,
            CatName = "Все категории"
        });

        filterCb.ItemsSource = categories;
        filterCb.SelectedIndex = 0;
    }

    private void LoadProd()
    {
        if (prodList == null || SortCb == null || searchTb == null || filterCb == null)
            return;

        try
        {
            using var db = new ChudoobuvContext();

            var query = db.Products
                .Include(p => p.Cat)
                .Include(p => p.Manuf)
                .Include(p => p.Stockitems)
                .Include(p => p.Ordersitems)
                .ThenInclude(o => o.Order)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTb.Text))
            {
                string text = searchTb.Text.ToLower();

                query = query.Where(p =>
                    (p.Descrip ?? "").ToLower().Contains(text) ||
                    (p.ProdName ?? "").ToLower().Contains(text));
            }

            if (filterCb.SelectedValue is int catId && catId > 0)
            {
                query = query.Where(p => p.CatId == catId);
            }

            var products = query.AsNoTracking().ToList();
            if (SortCb.SelectedIndex == 1)
                products = products.OrderBy(p => p.FinalPrice).ThenBy(p => p.IdProd).ToList();
            else if (SortCb.SelectedIndex == 2)
                products = products.OrderByDescending(p => p.FinalPrice).ThenBy(p => p.IdProd).ToList();
            else
                products = products.OrderBy(p => p.IdProd).ToList();
            prodList.ItemsSource = products;

        }
        catch(Exception ex)
        {
            MessageBox.Show("Ошибка загрузки:\n" + ex.Message, "ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void FiltersChanged(object sender, RoutedEventArgs e)
    {
        LoadProd();
    }
}
