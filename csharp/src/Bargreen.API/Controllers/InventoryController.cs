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


    // Note: Async/Await pattern is totally new to me.
    // I wanna try and do it but unfortunately I cannot test or run it because the computer that I'm using is a slow Macbook and
    // Visual Studio.Net doesnt work, and for this reason, I decided not to try async/await pattern.
    // so I only tried my best to code whatever to the very best of my knowledge without testing and debug my codes mentally...
    // Thank you.


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