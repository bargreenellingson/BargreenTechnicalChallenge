using Bargreen.API.Controllers;
using Bargreen.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryControllerTests
    {
        private readonly IInventoryService inventoryService = new InventoryService();
        [Fact]
        public async Task InventoryController_Can_Return_Inventory_BalancesAsync()
        {
            var controller = new InventoryController(inventoryService);
            var result = await controller.GetInventoryBalancesAsync();
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Controller_Methods_Are_Async()
        {
            var methods = typeof(InventoryController)
                .GetMethods()
                .Where(m=>m.DeclaringType==typeof(InventoryController));

            Assert.All(methods, m =>
            {
                Type attType = typeof(AsyncStateMachineAttribute); 
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }
    }
}
