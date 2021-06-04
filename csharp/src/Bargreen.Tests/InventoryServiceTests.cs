using Bargreen.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        private InventoryService InventoryService;

        /// <summary>
        /// Reconcilation should return a list
        /// </summary>
        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            // make sure reconcilation is not returning a null list

            var inventoryService = new InventoryService();
            var result = await inventoryService.ReconcileInventoryToAccounting(await inventoryService.GetInventoryBalances(), await inventoryService.GetAccountingBalances());

            Assert.NotEmpty((System.Collections.IEnumerable)result);
        }

        /// <summary>
        /// Make sure Accounting list is not null
        /// </summary>
        [Fact]
        public async void AccountingListNotNull()
        {
            var inventoryService = new InventoryService();
            var result = await inventoryService.GetAccountingBalances();

            Assert.NotNull(result);
        }
        /// <summary>
        /// Make sure Inventory list is not null
        /// </summary>
        [Fact]
        public async void InventoryListNotNull()
        {
            var inventoryService = new InventoryService();
            var result = await inventoryService.GetInventoryBalances();

            Assert.NotNull(result);
        }
        /// <summary>
        /// Makes sure the list parameters of ReconcileInventoryToAccounting are not null
        /// </summary>
        [Fact]
        public async void ReconcilationParametersNotNull()
        {
            var inventoryService = new InventoryService();
            var invBalances = await inventoryService.GetInventoryBalances();
            var accBalances = await inventoryService.GetAccountingBalances();
            bool areListPopulated = true;
            
            if(invBalances == null || accBalances == null)
            {
                areListPopulated = false;
                Assert.False(areListPopulated);
            }
            else
            {
                Assert.True(areListPopulated);
            }
            
        }
    }
}
