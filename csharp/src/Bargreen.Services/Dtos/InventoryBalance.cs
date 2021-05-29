using System;
using System.Collections.Generic;
using System.Text;

namespace Bargreen.Services.Dtos
{

    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
    }
}