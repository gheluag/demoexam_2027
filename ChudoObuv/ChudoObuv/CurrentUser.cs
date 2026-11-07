namespace ChudoObuv;

public static class CurrentUser
{
    public static int Id { get; set; }
    public static string FullName { get; set; } = "Гость";
    public static string Role { get; set; } = "Гость";
    public static bool IsAuthenticated => Id > 0;
    public static bool IsAdmin => Role == "Администратор";
    public static bool CanManageOrders => IsAdmin || Role == "Менеджер";

    public static void Reset()
    {
        Id = 0;
        FullName = "Гость";
        Role = "Гость";
        AppData.Cart.Clear();
    }
}
