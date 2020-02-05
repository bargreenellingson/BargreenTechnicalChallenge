using System.Collections.Generic;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bargreen.API.Controllers
{
    //TODO-CHALLENGE: Make the methods in this controller follow the async/await pattern: Done.
    //TODO-CHALLENGE: Use dotnet core dependency injection to inject the InventoryService: Done.
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await _inventoryService.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await _inventoryService.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            return await _inventoryService.ReconcileInventoryToAccounting(await _inventoryService.GetInventoryBalances(), await _inventoryService.GetAccountingBalances());
        }
    }
}