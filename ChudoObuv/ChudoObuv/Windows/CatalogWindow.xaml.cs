using ChudoObuv.DbContexts;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChudoObuv.Windows
{
    /// <summary>
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public CatalogWindow()
        {
            InitializeComponent();
            LoadProd();
            LoadCategories();
            LoadConfig();
            UserTb.Text = CurrentUser.FullName;
        }

        private void LoadConfig()
        {
            SortGrid.Visibility = 
                CurrentUser.Role != "Гость" ?
                Visibility.Visible :
                Visibility.Collapsed;
            orderBtn.Visibility =
                CurrentUser.Role != "Гость" ?
                Visibility.Visible :
                Visibility.Collapsed;
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

        private void UpdateCartButton()
        {
            if (orderBtn == null)
                return;

            orderBtn.Content = $"Корзина ({AppData.Cart.Sum(x => x.Quantity)})";
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var mn = new AuthWindow();
            mn.Show();
            Close();
        }

        private void LoadCategories()
        {
            using var db = new ChudoobuvContext();

            var categories = db.Categories.ToList();

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
                        p.Descrip.ToLower().Contains(text) ||
                        p.ProdName.ToLower().Contains(text));
                }

                if (filterCb.SelectedValue is int catId && catId > 0)
                {
                    query = query.Where(p => p.CatId == catId);
                }

                if (SortCb.SelectedIndex == 1)
                    query = query.OrderBy(p => p.Price);

                if (SortCb.SelectedIndex == 2)
                    query = query.OrderByDescending(p => p.Price);


                prodList.ItemsSource = query.ToList();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка загрузки:\n" + ex.Message, "ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProd();
        }

        private void filterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProd();
        }

        private void searchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProd();
        }

      
    }
}
