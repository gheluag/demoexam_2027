using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Ordersitem
{
    public int IdOrderItem { get; set; }

    public int? OrderId { get; set; }

    public int? ProdId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Prod { get; set; }
}
