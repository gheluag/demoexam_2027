using System;
using System.Collections.Generic;

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

    public int Count
    {
        get
        {
            return Stockitems.Sum(s => s.Quantity ?? 0);
        }
    }

    public string CountText
    {
        get
        {
            return Count > 5 ? "много" : "мало";
        }
    }

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
                o.Order.OrderDate >= previousMonthStart &&
                o.Order.OrderDate < currentMonthStart);
        }
    }

    public decimal FinalPrice
    {
        get
        {
            decimal price = Price ?? 0;

            return HasDiscount
                ? price * 0.75m
                : price;
        }
    }
}
