using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bargreen.API.Controllers
{
    //TODO-CHALLENGE: Make the methods in this controller follow the async/await pattern
    //TODO-CHALLENGE: Use dotnet core dependency injection to inject the InventoryService
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService invService)  
        {
            _inventoryService = invService;
        }
        
        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return _inventoryService.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return _inventoryService.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            return _inventoryService.ReconcileInventoryToAccounting(_inventoryService.GetInventoryBalances(), _inventoryService.GetAccountingBalances());
        }
    }
}