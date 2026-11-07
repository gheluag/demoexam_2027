using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Stockitem
{
    public int IdStockItem { get; set; }

    public int ProdId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public virtual Product? Prod { get; set; }

    public virtual Size? Size { get; set; }
}
