using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bargreen.Services
{

    //CHANGE class -> record, just to make it easier/clearer on its purpose. Immutable data
    public record InventoryReconciliationResult
    (
        string ItemNumber,
        decimal TotalValueOnHandInInventory,
        decimal TotalValueInAccountingBalance
    );

    public record InventoryBalance
    (
        string ItemNumber,
        string WarehouseLocation,
        int QuantityOnHand,
        decimal PricePerItem
    );

    public record AccountingBalance
    (
        string ItemNumber,
        decimal TotalInventoryValue
    );

    //CHANGE because DI/Testing/Mocking/Decoupling, interfaces should abstract the implementation
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync();
        Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync();

        private Dictionary<string, decimal> CompileDictionary<T>(IEnumerable<T> items, Func<T, ValueTuple<string, decimal>> kvFunc)
		{
            var dict = new Dictionary<string, decimal>();
            foreach(T item in items)
			{
                (string key, decimal value) = kvFunc(item);
                decimal currentValue = dict.GetValueOrDefault(key, 0);
                dict[key] = currentValue + value;
			}
            return dict;
		}

        //CHANGE made default interface method. Seems common between any implementation. Could easily just be another class method though
        public async Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccountingAsync()
        {
            //TODO issue with too much in memory?
            Task<IEnumerable<InventoryBalance>> inventoryTask = this.GetInventoryBalancesAsync();
            Task<IEnumerable<AccountingBalance>> accountingTask = this.GetAccountingBalancesAsync();
            await Task.WhenAll(inventoryTask, accountingTask);
            Dictionary<string, decimal> inventoryMap = this.CompileDictionary(inventoryTask.Result, i => (i.ItemNumber.ToLower(), i.QuantityOnHand * i.PricePerItem));
            Dictionary<string, decimal> accountingMap = this.CompileDictionary(accountingTask.Result, i => (i.ItemNumber.ToLower(), i.TotalInventoryValue));

           return inventoryMap.Keys
                .Concat(accountingMap.Keys)
                .Distinct()
                .Select(itemNumber => new InventoryReconciliationResult
                (
                    ItemNumber: itemNumber,
                    TotalValueOnHandInInventory: inventoryMap.GetValueOrDefault(itemNumber, 0),
                    TotalValueInAccountingBalance: accountingMap.GetValueOrDefault(itemNumber, 0)
                 ));
        }
    }

    public class InventoryService : IInventoryService
    {
        public Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync()
        {
            var balances = new List<InventoryBalance>()
            {
                new InventoryBalance
                (
                     ItemNumber: "ABC123",
                     PricePerItem: 7.5M,
                     QuantityOnHand: 312,
                     WarehouseLocation: "WLA1"
                ),
                new InventoryBalance
                (
                     ItemNumber: "ABC123",
                     PricePerItem: 7.5M,
                     QuantityOnHand: 146,
                     WarehouseLocation: "WLA2"
                ),
                new InventoryBalance
                (
                     ItemNumber: "ZZZ99",
                     PricePerItem: 13.99M,
                     QuantityOnHand: 47,
                     WarehouseLocation: "WLA3"
                ),
                new InventoryBalance
                (
                    //TODO item number case sensitive??
                     ItemNumber: "zzz99",
                     PricePerItem: 13.99M,
                     QuantityOnHand: 91,
                     WarehouseLocation: "WLA4"
                ),
                new InventoryBalance
                (
                     ItemNumber: "xxccM",
                     PricePerItem: 245.25M,
                     QuantityOnHand: 32,
                     WarehouseLocation: "WLA5"
                ),
                new InventoryBalance
                (
                     ItemNumber: "xxddM",
                     PricePerItem: 747.47M,
                     QuantityOnHand: 15,
                     WarehouseLocation: "WLA6"
                )
            };
            return Task.FromResult<IEnumerable<InventoryBalance>>(balances);
        }

        public Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync()
        {
            var balances = new List<AccountingBalance>()
            {
                new AccountingBalance("ABC123", 3435M),
                new AccountingBalance("ZZZ99", 1930.62M),
                new AccountingBalance("xxccM", 7602.75M),
                new AccountingBalance("fbr77", 17.99M)
            };
            return Task.FromResult<IEnumerable<AccountingBalance>>(balances);
        }
    }
}