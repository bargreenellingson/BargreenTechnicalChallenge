using Bargreen.Infrastructure;
using Bargreen.Services;
using Bargreen.Services.Interfaces;

namespace Bargreen.Tests
{
    public class InventoryTestsBase
    {
        protected readonly IInventoryService _inventoryService;
        protected readonly IInventoryRepository _inventoryRepository;

        public InventoryTestsBase()
        {
            _inventoryService = new InventoryService();
            _inventoryRepository = new InventoryRepository();
        }
    }
}