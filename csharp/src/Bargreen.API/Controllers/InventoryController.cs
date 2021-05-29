using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Bargreen.Services.Dtos;
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
        [Route("InventoryBalances")]
        [HttpGet]
        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            var inventoryService = new InventoryService();
            return inventoryService.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            var inventoryService = new InventoryService();
            return inventoryService.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public IEnumerable<InventoryReconciliationResult> GetReconciliation()
        {
            var inventoryService = new InventoryService();
            return InventoryService.ReconcileInventoryToAccounting(inventoryService.GetInventoryBalances(), inventoryService.GetAccountingBalances());
        }
    }
}