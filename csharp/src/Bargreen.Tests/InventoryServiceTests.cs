using System;
using Xunit;
using Bargreen.API;
using Bargreen.Services;
using System.Collections.Generic;
using System.Linq;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        IInventoryService inventoryService;
        [Fact]
        public void Inventory_Reconciliation_Performs_As_Expected_Base_Case()
        {
            inventoryService = new InventoryService();
            IEnumerable<InventoryBalance> inventoryList = inventoryService.GetInventoryBalances();
            IEnumerable<AccountingBalance> accountingList = inventoryService.GetAccountingBalances();

            IEnumerable<InventoryReconciliationResult> result = inventoryService.ReconcileInventoryToAccounting(inventoryList, accountingList);


            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count());
        }

        /**
         * this is a case where the inventoryOnHand is compared to the AccountingBalances which are empty
         * expected output should be exact copy of InventoryOnHandBalance
         * **/
        [Fact]
        public void Inventory_Reconciliation_No_Accounting()
        {
            List<InventoryBalance> inventoryOnHandBalances = new List<InventoryBalance>()
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
                }
            };
            inventoryService = new InventoryService(inventoryOnHandBalances);
            IEnumerable<InventoryBalance> inventoryList = inventoryService.GetInventoryBalances();
            IEnumerable<AccountingBalance> accountingList = inventoryService.GetAccountingBalances();

            IEnumerable<InventoryReconciliationResult> result = inventoryService.ReconcileInventoryToAccounting(inventoryList, accountingList);


            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }
    }
}
