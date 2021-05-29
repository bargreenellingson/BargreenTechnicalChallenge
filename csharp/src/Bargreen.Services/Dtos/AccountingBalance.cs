using System;
using System.Collections.Generic;
using System.Text;

namespace Bargreen.Services.Dtos
{

    public class AccountingBalance
    {
        public string ItemNumber { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }
}