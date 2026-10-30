using System;
using System.Collections.Generic;

namespace ChudoObuv.Entities;

public partial class Order
{
    public int IdOrder { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Ordersitem> Ordersitems { get; set; } = new List<Ordersitem>();

    public virtual User? User { get; set; }
}
