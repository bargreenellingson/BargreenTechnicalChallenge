using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bargreen.Services.Interfaces;

namespace Bargreen.API.Controllers
{
    //CHALLENGE: Make the methods in this controller follow the async/await pattern
    //CHALLENGE: Use dotnet core dependency injection to inject the InventoryService
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        IInventoryService invenService;

        public InventoryController(IInventoryService isvc)
        {
            invenService = isvc;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            //var inventoryService = new InventoryService();
            //return inventoryService.GetInventoryBalances();
            return await Task.Run< IEnumerable < InventoryBalance >>(()=> {
                return invenService.GetInventoryBalances();
                });
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            //var inventoryService = new InventoryService();
            //return inventoryService.GetAccountingBalances();
            return await Task.Run<IEnumerable<AccountingBalance>>(() =>
            {
                return invenService.GetAccountingBalances();
            });
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            //var inventoryService = new InventoryService();
            //return InventoryService.ReconcileInventoryToAccounting(inventoryService.GetInventoryBalances(), inventoryService.GetAccountingBalances());
            IEnumerable<InventoryBalance> invenBalance = await Task.Run<IEnumerable<InventoryBalance>>(() =>
            {
                return invenService.GetInventoryBalances();
            });

            IEnumerable<AccountingBalance> acctBalance = await Task.Run<IEnumerable<AccountingBalance>>(() =>
                {
                    return invenService.GetAccountingBalances();
                });

                return await Task.Run<IEnumerable<InventoryReconciliationResult>>(() =>
            {
                return invenService.ReconcileInventoryToAccounting(invenBalance, acctBalance);
            });
        }
    }
}