using ChudoObuv.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace ChudoObuv.Windows;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        CurrentUser.Reset();
    }

    private void OpenWindow()
    {
        var window = new CatalogWindow();
        Application.Current.MainWindow = window;
        window.Show();
        Close();
    }

    private void authBtn_Click(object sender, RoutedEventArgs e)
    {
        string login = loginTb.Text.Trim();
        if (string.IsNullOrWhiteSpace(login))
        {
            MessageBox.Show("Введите логин из списка пользователей.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        try
        {
            using var db = new ChudoobuvContext();
            var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == login);
            if (user?.Role == null)
            {
                MessageBox.Show("Пользователь с таким логином не найден или ему не назначена роль. Проверьте логин.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CurrentUser.Id = user.IdUser;
            CurrentUser.FullName = user.FullName;
            CurrentUser.Role = user.Role.RoleName ?? "";
            OpenWindow();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Не удалось подключиться к базе данных. Проверьте запуск MySQL и повторите вход.\n" + ex.Message, "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void guestTxt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        CurrentUser.Reset();
        OpenWindow();
    }
}
