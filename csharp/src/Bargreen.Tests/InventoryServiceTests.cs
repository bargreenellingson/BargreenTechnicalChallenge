using Bargreen.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public void ReconcileInventoryToAccounting_NoItems_ReturnsEmptyList()
        {
            var result = InventoryService.ReconcileInventoryToAccounting(new List<InventoryBalance>(), new List<AccountingBalance>());

            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]
        public void ReconcileInventoryToAccounting_MultipleInventoryRecordsForSameItemDifferentCase_ReturnsSum()
        {
            var inventoryBalances = new List<InventoryBalance>
            {
                new InventoryBalance
                {
                    ItemNumber = "a",
                    QuantityOnHand = 1,
                    PricePerItem = 5.00M,
                    WarehouseLocation = "x"
                },
                new InventoryBalance
                {
                    ItemNumber = "A",
                    QuantityOnHand = 2,
                    PricePerItem = 5.00M,
                    WarehouseLocation = "y"
                }
            };

            var accountingBalances = new List<AccountingBalance>();

            var result = InventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);

            Assert.NotNull(result);
            Assert.Single(result);

            var rec = result.First();

            Assert.Equal("A", rec.ItemNumber);
            Assert.Equal(0, rec.TotalValueInAccountingBalance);
            Assert.Equal(15M, rec.TotalValueOnHandInInventory);
        }

        [Fact]
        public void ReconcileInventoryToAccounting_AccountingRecordsNotInInventory_ReturnsZeroForInventoryValue()
        {
            var inventoryBalances = new List<InventoryBalance>();

            var accountingBalances = new List<AccountingBalance>
            {
                new AccountingBalance
                {
                    ItemNumber = "a",
                    TotalInventoryValue = 5M
                }
            };

            var result = InventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);

            Assert.NotNull(result);
            Assert.Single(result);

            var rec = result.First();

            Assert.Equal("A", rec.ItemNumber);
            Assert.Equal(5M, rec.TotalValueInAccountingBalance);
            Assert.Equal(0, rec.TotalValueOnHandInInventory);
        }

        [Fact]
        public void ReconcileInventoryToAccounting_InventoryRecordsNotInAccounting_ReturnsZeroForAccountingValue()
        {
            var inventoryBalances = new List<InventoryBalance>
            {
                new InventoryBalance
                {
                    ItemNumber = "a",
                    QuantityOnHand = 1,
                    PricePerItem = 5M,
                    WarehouseLocation = "x"
                }
            };

            var accountingBalances = new List<AccountingBalance>();

            var result = InventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);

            Assert.NotNull(result);
            Assert.Single(result);

            var rec = result.First();

            Assert.Equal("A", rec.ItemNumber);
            Assert.Equal(0, rec.TotalValueInAccountingBalance);
            Assert.Equal(5M, rec.TotalValueOnHandInInventory);
        }

        [Fact]
        public void ReconcileInventoryToAccounting_AccountingMatchesInventory_ResultMatches()
        {
            var inventoryBalances = new List<InventoryBalance>
            {
                new InventoryBalance
                {
                    ItemNumber = "a",
                    QuantityOnHand = 1,
                    PricePerItem = 5.00M,
                    WarehouseLocation = "x"
                },
                new InventoryBalance
                {
                    ItemNumber = "A",
                    QuantityOnHand = 2,
                    PricePerItem = 5.00M,
                    WarehouseLocation = "y"
                }
            };

            var accountingBalances = new List<AccountingBalance>
            {
                new AccountingBalance
                {
                    ItemNumber = "a",
                    TotalInventoryValue = 15M
                }
            };

            var result = InventoryService.ReconcileInventoryToAccounting(inventoryBalances, accountingBalances);

            Assert.NotNull(result);
            Assert.Single(result);

            var rec = result.First();

            Assert.Equal("A", rec.ItemNumber);
            Assert.Equal(15M, rec.TotalValueInAccountingBalance);
            Assert.Equal(15M, rec.TotalValueOnHandInInventory);
        }
    }
}
