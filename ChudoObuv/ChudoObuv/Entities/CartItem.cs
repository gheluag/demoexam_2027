using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChudoObuv.Entities
{
    public class CartItem
    {
        public int ProdId { get; set; }

        public int StockItemId { get; set; }

        public string ProdName { get; set; } = "";

        public string ManufName { get; set; } = "";

        public string SizeName { get; set; } = "";

        public int Quantity { get; set; }

        public int MaxQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Sum => Quantity * UnitPrice;
    }
}
