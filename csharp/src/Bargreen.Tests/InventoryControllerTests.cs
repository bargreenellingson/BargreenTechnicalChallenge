using Bargreen.API.Controllers;
using Bargreen.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryControllerTests
    {
        //CHANGE added DI and mocked the depenedency 
        [Fact]
        public async Task InventoryController_Can_Return_Inventory_Balances()
        {
            var inventoryService = new Mock<IInventoryService>();
            var data = new List<InventoryBalance>();
            inventoryService
                .Setup(i => i.GetInventoryBalancesAsync())
                .ReturnsAsync(data);
            var controller = new InventoryController(inventoryService.Object);
            var result = await controller.GetInventoryBalances();
            Assert.Same(data, result);
        }

        [Fact]
        public void Controller_Methods_Are_Async()
        {
            var methods = typeof(InventoryController)
                .GetMethods()
                .Where(m=>m.DeclaringType==typeof(InventoryController));

            Assert.All(methods, m =>
            {
                //TODO only detects async, not if returns Task
                Type attType = typeof(AsyncStateMachineAttribute); 
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }
    }
}
