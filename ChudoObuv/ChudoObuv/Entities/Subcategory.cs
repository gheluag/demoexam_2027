using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Subcategory
{
    public int IdSubCat { get; set; }

    public string? SubCatName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
