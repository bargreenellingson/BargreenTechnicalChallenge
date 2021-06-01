using Bargreen.Services.Dtos;
using Bargreen.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargreen.Services
{
    public class InventoryService : IInventoryService
    {
        public IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences
            Dictionary<string, Decimal[]> result = new Dictionary<string, Decimal[]>();

            foreach (var inv in inventoryBalances)
            {
                if (!result.ContainsKey(inv.ItemNumber.ToUpper()))
                {
                    result.Add(inv.ItemNumber, new decimal[] { inv.PricePerItem * inv.QuantityOnHand, 0.00M });
                }
                else
                {
                    result[inv.ItemNumber.ToUpper()][0] += inv.PricePerItem * inv.QuantityOnHand;
                }
            }

            foreach (var act in accountingBalances)
            {
                if (result.ContainsKey(act.ItemNumber))
                {
                    result[act.ItemNumber][1] = act.TotalInventoryValue;
                }
                else
                {
                    result.Add(act.ItemNumber, new decimal[] { 0.00M, act.TotalInventoryValue });
                }
            }

            return result.Select(r => new InventoryReconciliationResult
            {
                ItemNumber = r.Key,
                TotalValueOnHandInInventory = r.Value[0],
                TotalValueInAccountingBalance = r.Value[1]
            }).ToList();
        }
    }
}