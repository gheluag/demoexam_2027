namespace ChudoObuv.Entities;

public partial class Product
{
    public int IdProd { get; set; }

    public int? CatId { get; set; }

    public int? SubCatId { get; set; }

    public string? Image { get; set; }

    public string? ProdName { get; set; }

    public int? ManufId { get; set; }

    public string? Descrip { get; set; }

    public string? Struct { get; set; }

    public decimal? Price { get; set; }

    public virtual Category? Cat { get; set; }

    public virtual Manufacrure? Manuf { get; set; }

    public virtual ICollection<Ordersitem> Ordersitems { get; set; } = new List<Ordersitem>();

    public virtual ICollection<Stockitem> Stockitems { get; set; } = new List<Stockitem>();

    public virtual Subcategory? SubCat { get; set; }

    public int Count => Stockitems.Sum(s => s.Quantity);

    public string CountText => Count > 5 ? "много" : "мало";

    public bool IsLowStock => Count <= 3;

    public bool HasDiscount
    {
        get
        {
            DateTime currentMonthStart =
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            DateTime previousMonthStart =
                currentMonthStart.AddMonths(-1);

            return !Ordersitems.Any(o =>
                o.Order != null && o.Order.OrderDate >= previousMonthStart &&
                o.Order.OrderDate < currentMonthStart);
        }
    }

    public decimal FinalPrice
    {
        get
        {
            decimal price = Price ?? 0;

            if (HasDiscount)
                return decimal.Round(price * 0.75m, 2, MidpointRounding.AwayFromZero);
            return price;
        }
    }
}
