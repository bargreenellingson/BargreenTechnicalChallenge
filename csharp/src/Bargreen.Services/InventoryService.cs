using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


    public interface InventoryServiceInterface
    {
        Task<IEnumerable<InventoryBalance>> GetInventoryBalances();

        Task<IEnumerable<AccountingBalance>> GetAccountingBalances();
    }

    public class InventoryService : InventoryServiceInterface
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return new List<InventoryBalance>()
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
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
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

        public static IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            // assumptions: item numbers are case-insensitive; accounting balance list does not contain duplicate item numbers

            // get reconciliation result for all item numbers in accounting balance list
            // note: calling Sum() on an empty list will return 0
            var acc = accountingBalances.Select(a => new InventoryReconciliationResult
            {
                ItemNumber = a.ItemNumber.ToUpper(),
                TotalValueInAccountingBalance = a.TotalInventoryValue,
                TotalValueOnHandInInventory = inventoryBalances
                    .Where(i => string.Equals(i.ItemNumber, a.ItemNumber, StringComparison.OrdinalIgnoreCase))
                    .Select(n => n.PricePerItem * n.QuantityOnHand)
                    .Sum()
            });

            // get item numbers we have already added to the list
            var itemNumbersInList = acc.Select(a => a.ItemNumber);

            // get reconciliation result for item numbers in inventory balance list that have not already been processed
            var inv = inventoryBalances
                .Where(i => !itemNumbersInList.Contains(i.ItemNumber.ToUpper()))
                .GroupBy(num => num.ItemNumber.ToUpper(), balance => balance, (num, balances) => new InventoryReconciliationResult
            {
                ItemNumber = num,
                TotalValueInAccountingBalance = 0,
                TotalValueOnHandInInventory = balances.Select(n => n.PricePerItem * n.QuantityOnHand).Sum()
            });

            // concatenate the two lists
            return acc.Concat(inv);
        }
    }
}
