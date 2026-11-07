using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Ordersitem
{
    public int IdOrderItem { get; set; }

    public int? OrderId { get; set; }

    public int? ProdId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Size? Size { get; set; }

    public decimal Sum => Quantity * UnitPrice;

    public string ModelName => $"{Prod?.Manuf?.ManufName} | {Prod?.ProdName}";

    public string SizeName => Size?.Size1 ?? "Нет исходных данных";

    public virtual Order? Order { get; set; }

    public virtual Product? Prod { get; set; }
}
