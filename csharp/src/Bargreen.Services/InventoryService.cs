using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bargreen.Services.Interfaces;
using System.Linq;

namespace Bargreen.Services 
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }
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


    public class InventoryService : IInventoryService
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            List<InventoryBalance> bal = await Task.Run<List<InventoryBalance>>(() =>
           {
               return new List<InventoryBalance>
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
           };
           });
                return bal;
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await Task.Run<IEnumerable<AccountingBalance>>(() =>
            {
                return new List<AccountingBalance>
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
            };
            });
        }

         async  Task<IEnumerable<InventoryReconciliationResult>> IInventoryService.ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            return await ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);
        }


         async static Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //CHALLENGE: Compare inventory balances to accounting balances and find differences
            return await Task.Run<IEnumerable<InventoryReconciliationResult>>(() =>
            {
            List<InventoryReconciliationResult> differences = new List<InventoryReconciliationResult>();

                //check for accounts without matching inventories
                foreach (AccountingBalance acct in accountingBalances)
                {
                    var matchCount = (from a in inventoryBalances where acct.ItemNumber == a.ItemNumber select a).Count();
                    if (matchCount==0)
                    {
                        differences.Add(new InventoryReconciliationResult
                        {
                            ItemNumber = acct.ItemNumber,
                            TotalValueOnHandInInventory = 0,
                            TotalValueInAccountingBalance = acct.TotalInventoryValue
                        });
                    }
                }

                //batch up items by itemNumber
                List<InventoryBalance> invenSummary = new List<InventoryBalance>();
                foreach (string itemNumber in (inventoryBalances.Select(i=>i.ItemNumber).Distinct(StringComparer.CurrentCultureIgnoreCase)))
                {
                    invenSummary.Add(new InventoryBalance
                    {
                        ItemNumber = itemNumber,
                        QuantityOnHand = (from i in inventoryBalances where String.Equals(i.ItemNumber ,itemNumber, StringComparison.CurrentCultureIgnoreCase) select i.QuantityOnHand).Sum(),
                        PricePerItem = (from i in inventoryBalances where String.Equals(i.ItemNumber, itemNumber, StringComparison.CurrentCultureIgnoreCase) select i.PricePerItem).First()
                    }) ;
                }

                //get inventory listings where balance doesn't match accounts table,
                //or item is missing from accounts table
                foreach (InventoryBalance inventoryItem in invenSummary)
                {
                AccountingBalance accountingItem = accountingBalances.FirstOrDefault(bal => bal.ItemNumber == inventoryItem.ItemNumber);
                    decimal valueInInventory = inventoryItem.QuantityOnHand * inventoryItem.PricePerItem;
                    if (accountingItem == null)
                    {
                        differences.Add(new InventoryReconciliationResult
                        {
                            ItemNumber = inventoryItem.ItemNumber,
                            TotalValueOnHandInInventory = valueInInventory,
                            TotalValueInAccountingBalance = 0
                        });
                    }
                    else if (valueInInventory != accountingItem.TotalInventoryValue)
                    {
                        differences.Add(new InventoryReconciliationResult
                        {
                            ItemNumber = inventoryItem.ItemNumber,
                            TotalValueOnHandInInventory = valueInInventory,
                            TotalValueInAccountingBalance = accountingItem.TotalInventoryValue
                        });
                    }
                }

                return differences;
            });
        }
    }
}