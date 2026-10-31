using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChudoObuv.Entities;

namespace ChudoObuv
{
    public static class AppData
    {

        public static List<CartItem> Cart { get; set; } = new();
    }
}
