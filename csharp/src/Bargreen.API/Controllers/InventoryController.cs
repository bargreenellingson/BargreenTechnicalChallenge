using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
        private InventoryService service;
        
        public InventoryController(InventoryService service)
        {
            this.service = service;
        }
        
        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await service.GetInventoryBalancesAsync();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await service.GetAccountingBalancesAsync();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            return await InventoryService.ReconcileInventoryToAccounting(await  service.GetInventoryBalancesAsync(), await service.GetAccountingBalancesAsync());
        }
    }
}