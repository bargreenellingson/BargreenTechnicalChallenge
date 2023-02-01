using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal? TotalValueOnHandInInventory { get; set; }
        public decimal? TotalValueInAccountingBalance { get; set; }
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

    // By creating an interface and implementing it here, we can perform dependency injection and avoid tightly
    // coupled classes.
    public class InventoryService : IInventoryService
    {
        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            // The code here is naturally synchronous, so we're forced to keep it that way.
            return new List<InventoryBalance>() {
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
                }};
        }

        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            // The code here is naturally synchronous, so we're forced to keep it that way.
            return new List<AccountingBalance>()
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
        }

        public IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences

            // Here we're generating the actual total inventory value for items in our inventory
            // With grouping by item number, we're ensuring that items are calculated for in all regions.
            List<AccountingBalance> totalValueOnHand = inventoryBalances.GroupBy(x => x.ItemNumber)
                                                               .Select(invBalance => new AccountingBalance
                                                               {
                                                                   ItemNumber = invBalance.First().ItemNumber,
                                                                   TotalInventoryValue = invBalance.Sum(ib => ib.PricePerItem * ib.QuantityOnHand)
                                                               }).ToList();


            List<InventoryReconciliationResult> inventoryReconciliationResults = new List<InventoryReconciliationResult>();

            // We will use this list to reconcile item numbers existing in Inventory but not in Accounting balance data set later.
            List<AccountingBalance> accountingBalancesList = accountingBalances.ToList();
            List<string> missingItemNumbersFromAccountingBalances = totalValueOnHand.Where(ac => !accountingBalancesList.Exists(ab => ab.ItemNumber == ac.ItemNumber)).Select(a => a.ItemNumber).ToList();

            // Here we will generate our result list by doing comparisons and checking if list contains item.
            foreach (AccountingBalance ab in accountingBalances)
            {
                if (totalValueOnHand.Exists(accBal => accBal.ItemNumber == ab.ItemNumber))
                {
                    inventoryReconciliationResults.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = ab.ItemNumber,
                        TotalValueInAccountingBalance = ab.TotalInventoryValue,
                        TotalValueOnHandInInventory = totalValueOnHand.First(accBal => accBal.ItemNumber == ab.ItemNumber).TotalInventoryValue
                    });
                } 
                // This else statement will create records for account balances with item numbers that don't exist
                // in our inventory data set.
                else
                {
                    inventoryReconciliationResults.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = ab.ItemNumber,
                        TotalValueInAccountingBalance = ab.TotalInventoryValue,
                        TotalValueOnHandInInventory = null
                    });
                }
            }

            // finally for item numbers not in accounting balance data set, create a record as well
            foreach (string itemNumber in missingItemNumbersFromAccountingBalances)
            {
                inventoryReconciliationResults.Add(new InventoryReconciliationResult()
                {
                    ItemNumber = itemNumber,
                    TotalValueInAccountingBalance = null,
                    TotalValueOnHandInInventory = totalValueOnHand.First(ac => ac.ItemNumber == itemNumber).TotalInventoryValue
                });
            }

            return inventoryReconciliationResults;
        }
    }
}