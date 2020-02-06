using Bargreen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        // Test that Async has been acheived in all methods.
        public void Service_Methods_Are_Async()
        {
            var methods = typeof(InventoryService)
                .GetMethods()
                .Where(m => m.DeclaringType == typeof(InventoryService));

            Assert.All(methods, m =>
            {
                Type attType = typeof(AsyncStateMachineAttribute);
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }

        [Fact]
        // There are no parameters to pass, so the method needs to return a non-null List<InventoryBalance>.
        public async void GetInventoryBalance_Performs_As_Expected()
        {
            var service = new InventoryService();
            var result = await service.GetInventoryBalances();
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryBalance>>(result); // Perhaps unnecessary in a strongly typed language?  I have spent a long time in Javascript land.
        }

        [Fact]
        // There are no parameters to pass, so the method needs to return a non-null List<InventoryBalance>.
        public async void GetAccountingBalance_Performs_As_Expected()
        {
            var service = new InventoryService();
            var result = await service.GetAccountingBalances();
            Assert.NotEmpty(result);
            Assert.IsType<List<AccountingBalance>>(result);
        }

        // Two Parameters, we need test what happens when either are null or empty and that proper input produces proper results.
        [Fact]
        public async void Null_Inventory_List_Throws_Error()
        {
            var service = new InventoryService();
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await service.ReconcileInventoryToAccounting(null, await service.GetAccountingBalances()));
        }

        [Fact]
        public async void Null_Accounting_List_Throws_Error()
        {
            var service = new InventoryService();
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), null));
        }

        [Fact]
        public async void Empty_Inventory_List_Returns_Accounting_Results()
        {
            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(new List<InventoryBalance>(), await service.GetAccountingBalances());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
        }

        [Fact]
        public async void Empty_Accounting_List_Returns_Inventory_Results()
        {
            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), new List<AccountingBalance>());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
        }

        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), await service.GetAccountingBalances());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
        }
    }
}
