using System;
using Xunit;
using Bargreen.Services;
using Bargreen.Services.Interfaces;
using Bargreen.API.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            //CHALLENGE: Verify expected output of your recon algorithm. Note, this will probably take more than one test
            
            IInventoryService svc = new InventoryService();
            var controller = new InventoryController(svc);

            var discrepanciesList =(List<InventoryReconciliationResult>) await Task.Run( () =>
            {
                var result = controller.GetReconciliation();
                return result;
            });

            //check results are as expected
            Assert.Equal(3, discrepanciesList.Count);
            Assert.True(isItemContained(discrepanciesList, "fbr77"));
            Assert.True(isItemContained(discrepanciesList, "xxccM"));
            Assert.True(isItemContained(discrepanciesList, "xxddM"));

        }

        private Boolean isItemContained(List<InventoryReconciliationResult> itemList, string itemNumber)
        {
            var matchCount = (from d in itemList where String.Equals(d.ItemNumber, itemNumber, StringComparison.CurrentCultureIgnoreCase) select d).Count();
            return (matchCount > 0);
        }
    }
}
