using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Size
{
    public int IdSize { get; set; }

    public string? Size1 { get; set; }

    public virtual ICollection<Stockitem> Stockitems { get; set; } = new List<Stockitem>();
}
