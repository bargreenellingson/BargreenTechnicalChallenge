using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Bargreen.API.Controllers
{
    //TODO-CHALLENGE: Make the methods in this controller follow the async/await pattern
    //TODO-CHALLENGE: Use dotnet core dependency injection to inject the InventoryService
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        
		private readonly IInventoryService _inventoryService;

        // Added a parameterized constructor for dependency injection. Now instead of creating the object of Inventory Service class we are injecting the 
        //depenndency.
        //This  will help in achieving Single Responsibility Principle as well as now each method is only responsible for its assigned task. 
        //It is no longer responsible for object creation.
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Route("InventoryBalances")]
        [HttpGet]

        //Applying Async/await pattern on all methods in this controller. Changed all synchronous methods to asynchronous methods.
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await Task.Run(() => (_inventoryService.GetInventoryBalances()));
        }
        

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
             var Accounting =  Task.Run(()=> (_inventoryService.GetAccountingBalances()));
             return await Accounting;
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            return  await Task.Run(() => (_inventoryService.ReconcileInventoryToAccounting(_inventoryService.GetInventoryBalances(), 
            _inventoryService.GetAccountingBalances())));
        }
    }
}