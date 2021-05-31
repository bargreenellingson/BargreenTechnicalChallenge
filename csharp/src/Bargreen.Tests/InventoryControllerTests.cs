using Bargreen.API.Controllers;
using Bargreen.Services.Interfaces;
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
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryControllerTests(IInventoryService inventoryService, IInventoryRepository inventoryRepository)
        {
            _inventoryService = inventoryService;
            _inventoryRepository = inventoryRepository;
        }

        [Fact]
        public void InventoryController_Can_Return_Inventory_Balances()
        {
            var controller = new InventoryController(_inventoryService, _inventoryRepository);
            var result = controller.GetInventoryBalances();
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
