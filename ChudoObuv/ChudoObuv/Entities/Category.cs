using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Category
{
    public int IdCat { get; set; }

    public string? CatName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
