using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }
        
        public decimal InventoryAccountingDifference { get; set; }
    }

    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
    }

    public class AccountingBalance
    {
        public string ItemNumber { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }


    public class InventoryService
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync()
        {
            //An Async Database call would be here, awaited, then res would be returned
            var res  = await Task.Run(() =>new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 312,
                     WarehouseLocation = "WLA1"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 146,
                     WarehouseLocation = "WLA2"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ZZZ99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 47,
                     WarehouseLocation = "WLA3"
                },
                new InventoryBalance()
                {
                     ItemNumber = "zzz99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 91,
                     WarehouseLocation = "WLA4"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxccM",
                     PricePerItem = 245.25M,
                     QuantityOnHand = 32,
                     WarehouseLocation = "WLA5"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxddM",
                     PricePerItem = 747.47M,
                     QuantityOnHand = 15,
                     WarehouseLocation = "WLA6"
                }
            });

            return res;
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync()
        {
            //An Async Database call would be here, awaited, then res would be returned
            var res  = await Task.Run(() => new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3435M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbr77",
                     TotalInventoryValue = 17.99M
                }
            });

            return res;
        }

        public async static Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences
            var reconciliationList = new List<InventoryReconciliationResult>();
            
            if (inventoryBalances.Any() && accountingBalances.Any())
            {
                var accountingList = accountingBalances.ToList();
                foreach (var inv in inventoryBalances)
                {
                    InventoryReconciliationResult reconRes;
                    //Check to see if there are duplicate inv items
                    if (reconciliationList.Any(r => r.ItemNumber == inv.ItemNumber.ToUpper()))
                    {
                        reconRes = reconciliationList.Find(r => r.ItemNumber == inv.ItemNumber.ToUpper());
                        reconRes.TotalValueOnHandInInventory += inv.PricePerItem * inv.QuantityOnHand;
                    }
                    else
                    {
                        reconRes = new InventoryReconciliationResult
                        {
                            ItemNumber = inv.ItemNumber.ToUpper(),
                            TotalValueOnHandInInventory = inv.PricePerItem * inv.QuantityOnHand
                        };
                        reconciliationList.Add(reconRes);
                    }

                    if (accountingList.Any(a => a.ItemNumber.ToUpper() == reconRes.ItemNumber))
                    {
                        reconRes.TotalValueInAccountingBalance = accountingList
                            .Find(a => a.ItemNumber.ToUpper() == reconRes.ItemNumber)
                            .TotalInventoryValue;
                    }
                    else
                    {
                        reconRes.TotalValueInAccountingBalance = 0;
                    }

                    reconRes.InventoryAccountingDifference =
                        reconRes.TotalValueOnHandInInventory - reconRes.TotalValueInAccountingBalance;
                }
            }
            else if(!accountingBalances.Any())
            {
                foreach (var inv in inventoryBalances)
                {
                    InventoryReconciliationResult reconRes;
                    //Check to see if there are duplicate inv items
                    if (reconciliationList.Any(r => r.ItemNumber == inv.ItemNumber.ToUpper()))
                    {
                        reconRes = reconciliationList.Find(r => r.ItemNumber == inv.ItemNumber.ToUpper());
                        reconRes.TotalValueOnHandInInventory += inv.PricePerItem * inv.QuantityOnHand;
                    }
                    else
                    {
                        reconRes = new InventoryReconciliationResult
                        {
                            ItemNumber = inv.ItemNumber.ToUpper(),
                            TotalValueOnHandInInventory = inv.PricePerItem * inv.QuantityOnHand
                        };
                        reconciliationList.Add(reconRes);
                    }
                    reconRes.InventoryAccountingDifference =
                        reconRes.TotalValueOnHandInInventory - reconRes.TotalValueInAccountingBalance;
                }
            }

            if (accountingBalances.Count() != 0)
            {
                foreach (var balance in accountingBalances)
                {
                    if (reconciliationList.All(a => a.ItemNumber != balance.ItemNumber.ToUpper()))
                    {
                        var reconRes = new InventoryReconciliationResult();
                        reconRes.ItemNumber = balance.ItemNumber.ToUpper();
                        reconRes.TotalValueOnHandInInventory = 0;
                        reconRes.TotalValueInAccountingBalance = balance.TotalInventoryValue;
                        reconRes.InventoryAccountingDifference =
                            reconRes.TotalValueOnHandInInventory - reconRes.TotalValueInAccountingBalance;
                        reconciliationList.Add(reconRes);
                    }
                }
            }

            return reconciliationList;
        }
    }
}