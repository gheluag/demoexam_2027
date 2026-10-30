using ChudoObuv.DbContexts;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChudoObuv.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void OpenWindow()
        {
            CatalogWindow catWin = new();
            catWin.Show();
            Close();
        }

        private void authBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTb.Text.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("введите логин", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var db = new ChudoobuvContext();

            var user = db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login);

            if(user == null)
            {
                MessageBox.Show("неверный логин", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentUser.Id = user.IdUser;
            CurrentUser.FullName = $"{user.LastName} {user.FirstName} {user.MiddleName}";
            CurrentUser.Role = user.Role.RoleName;
            OpenWindow();

        }


        private void guestTxt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentUser.Id = 0;
            CurrentUser.FullName = "Гость";
            CurrentUser.Role = "Гость";
            OpenWindow();

        }
    }
}