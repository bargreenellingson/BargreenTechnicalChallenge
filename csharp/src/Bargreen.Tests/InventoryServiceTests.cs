using Bargreen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public async Task Inventory_Reconciliation_Performs_As_Expected()
        {
            //Assuming itemNumber is case insensitive
            var expected = new List<InventoryReconciliationResult>
            {
                new InventoryReconciliationResult("abc123", 3435.0m, 3435),
                new InventoryReconciliationResult("zzz99", 1930.62m, 1930.62m),
                new InventoryReconciliationResult("xxccm", 7848.00m, 7602.75m),
                new InventoryReconciliationResult("xxddm", 11212.05m, 0),
                new InventoryReconciliationResult("fbr77", 0m, 17.99m)
            };
            IInventoryService inventoryService = new InventoryService();
			IEnumerable<InventoryReconciliationResult> result = await inventoryService.ReconcileInventoryToAccountingAsync();
            Assert.Equal(expected, result.ToList());
        }
    }
}
