using Bargreen.Services;
using Bargreen.Services.Dtos;
using Bargreen.Services.Interfaces;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests : InventoryTestsBase
    {
        private readonly IEnumerable<InventoryReconciliationResult> _expectedResults;
        public InventoryServiceTests()
        {
            _expectedResults = new List<InventoryReconciliationResult>()
            {
                new InventoryReconciliationResult()
                {
                    ItemNumber = "ABC123",
                    TotalValueOnHandInInventory = 3435.0M,
                    TotalValueInAccountingBalance = 3435
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "ZZZ99",
                    TotalValueOnHandInInventory = 1930.62M,
                    TotalValueInAccountingBalance =1930.62M
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "xxccM",
                    TotalValueOnHandInInventory = 7848.00M,
                    TotalValueInAccountingBalance =7602.75M
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "xxddM",
                    TotalValueOnHandInInventory = 11212.05M,
                    TotalValueInAccountingBalance = 0.00M
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "fbr77",
                    TotalValueOnHandInInventory = 0.00M,
                    TotalValueInAccountingBalance = 17.99M
                },
            };
        }

        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            //TODO-CHALLENGE: Verify expected output of your recon algorithm. Note, this will probably take more than one test

            IInventoryService _inventoryService = new InventoryService();
            IEnumerable<InventoryReconciliationResult> actualResults = await _inventoryService.ReconcileInventoryToAccounting(await _inventoryRepository.GetInventoryBalances(), await _inventoryRepository.GetAccountingBalances());

            actualResults.Should().BeEquivalentTo(_expectedResults);
        }

        [Fact]
        public async void Inventory_Reconciliation_Should_Fail_With_Bad_Data()
        {
            IEnumerable<AccountingBalance> badAccountingList = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3535M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbT77",
                     TotalInventoryValue = 17.99M
                }
            };

            IInventoryService _inventoryService = new InventoryService();
            IEnumerable<InventoryReconciliationResult> actualResults = await _inventoryService.ReconcileInventoryToAccounting(await _inventoryRepository.GetInventoryBalances(), badAccountingList);

            actualResults.Should().NotBeEquivalentTo(_expectedResults);
        }
    }
}
