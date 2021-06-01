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
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryService inventoryService, IInventoryRepository inventoryRepository)
        {
            _inventoryService = inventoryService;
            _inventoryRepository = inventoryRepository;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await _inventoryRepository.GetInventoryBalances();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await _inventoryRepository.GetAccountingBalances();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            return await _inventoryService.ReconcileInventoryToAccounting(await _inventoryRepository.GetInventoryBalances(), await _inventoryRepository.GetAccountingBalances());
        }
    }
}