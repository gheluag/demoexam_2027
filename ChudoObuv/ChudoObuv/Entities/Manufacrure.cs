using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Manufacrure
{
    public int IdManuf { get; set; }

    public string? ManufName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
