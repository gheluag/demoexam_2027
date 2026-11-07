namespace ChudoObuv.Entities;

public partial class Order
{
    public int IdOrder { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Ordersitem> Ordersitems { get; set; } = new List<Ordersitem>();

    public virtual User? User { get; set; }

    public string ClientName => User?.FullName ?? "";

    public decimal Total => Ordersitems.Sum(x => x.Sum);

    public string TotalText => $"{Total:F2} ₽";
}
