using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Bargreen.API.Controllers
{
    //TODO-CHALLENGE: Make the methods in this controller follow the async/await pattern
    //TODO-CHALLENGE: Use dotnet core dependency injection to inject the InventoryService

    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        // Here we're using Constructor dependency injection type. It allows our controller object to access
        // the implementation we selected in Startup upon creation.
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync()
        {
            // Here we see a slightly different implementation from before.
            // First we're using dependency injection to access our inventory service, second we're using Task.Run
            // Our service uses synchronous code to retrieve a balance list, and due to that I was forced to use Task.Run
            // Task.Run is a bad practice especially in Web APIs for multitude of reasons
            // Reasons being reducing performance/scalability by blocking threadpool, potentially causing deadlocks, etc.
            // However due to lack of naturally asynchronous code such as using a database context framework or a
            // streamreader, to convert our APIs to use the await/async pattern, I used Task.Run
            // It unblocks the current thread by offloading this task to another thread and allows for asynchronous execution.
            // In a real world implementation using something such as Entity Framework, we don't need Task.Run.
            return await Task.Run(() => { return _inventoryService.GetInventoryBalances(); });
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync()
        {
            return await Task.Run(() => { return _inventoryService.GetAccountingBalances(); });
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliationAsync()
        {
            return await Task.Run(async () => { return _inventoryService.ReconcileInventoryToAccounting(await GetInventoryBalancesAsync(), await GetAccountingBalancesAsync()); });
        }
    }
}