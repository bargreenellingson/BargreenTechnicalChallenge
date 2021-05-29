using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Bargreen.Services.Dtos;
using Bargreen.Services.Interfaces;
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

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            var inventoryService = new InventoryService();
            return _inventoryService.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            var inventoryService = new InventoryService();
            return _inventoryService.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public IEnumerable<InventoryReconciliationResult> GetReconciliation()
        {
            var inventoryService = new InventoryService();
            return _inventoryService.ReconcileInventoryToAccounting(inventoryService.GetInventoryBalances(), inventoryService.GetAccountingBalances());
        }
    }
}