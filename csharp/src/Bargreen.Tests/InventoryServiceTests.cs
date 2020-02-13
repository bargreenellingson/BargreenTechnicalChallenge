using Bargreen.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        // Test that Async has been acheived in all methods.
        public void Service_Methods_Are_Async()
        {
            var methods = typeof(InventoryService)
                .GetMethods()
                .Where(m => m.DeclaringType == typeof(InventoryService));

            Assert.All(methods, m =>
            {
                Type attType = typeof(AsyncStateMachineAttribute);
                var attrib = (AsyncStateMachineAttribute)m.GetCustomAttribute(attType);
                Assert.NotNull(attrib);
                Assert.Equal(typeof(Task), m.ReturnType.BaseType);
            });
        }

        [Fact]
        // There are no parameters to pass, so the method needs to return a non-null List<InventoryBalance>.
        public async void GetInventoryBalance_Performs_As_Expected()
        {
            IEnumerable<InventoryBalance> test = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                        ItemNumber = "ABC123",
                        PricePerItem = 7.5M,
                        QuantityOnHand = 312,
                        WarehouseLocation = "WLA1"
                },
                new InventoryBalance()
                {
                        ItemNumber = "ABC123",
                        PricePerItem = 7.5M,
                        QuantityOnHand = 146,
                        WarehouseLocation = "WLA2"
                },
                new InventoryBalance()
                {
                        ItemNumber = "ZZZ99",
                        PricePerItem = 13.99M,
                        QuantityOnHand = 47,
                        WarehouseLocation = "WLA3"
                },
                new InventoryBalance()
                {
                        ItemNumber = "zzz99",
                        PricePerItem = 13.99M,
                        QuantityOnHand = 91,
                        WarehouseLocation = "WLA4"
                },
                new InventoryBalance()
                {
                        ItemNumber = "xxccM",
                        PricePerItem = 245.25M,
                        QuantityOnHand = 32,
                        WarehouseLocation = "WLA5"
                },
                new InventoryBalance()
                {
                        ItemNumber = "xxddM",
                        PricePerItem = 747.47M,
                        QuantityOnHand = 15,
                        WarehouseLocation = "WLA6"
                }
            };
            var service = new InventoryService();
            var result = await service.GetInventoryBalances();
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryBalance>>(result); // Perhaps unnecessary in a strongly typed language?  I have spent a long time in Javascript land.
            Assert.Equal(6, result.Count());
            Assert.True(JsonConvert.SerializeObject(result).Equals(JsonConvert.SerializeObject(test)));
        }

        [Fact]
        // There are no parameters to pass, so the method needs to return a non-null List<InventoryBalance>.
        public async void GetAccountingBalance_Performs_As_Expected()
        {
            IEnumerable<AccountingBalance> test = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                        ItemNumber = "ABC123",
                        TotalInventoryValue = 3435M
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
                        ItemNumber = "fbr77",
                        TotalInventoryValue = 17.99M
                }
            };
            var service = new InventoryService();
            var result = await service.GetAccountingBalances();
            Assert.NotEmpty(result);
            Assert.IsType<List<AccountingBalance>>(result);
            Assert.Equal(4, result.Count());
            Assert.True(JsonConvert.SerializeObject(result).Equals(JsonConvert.SerializeObject(test)));
        }

        // Two Parameters, we need test what happens when either are null or empty and that proper input produces proper results.
        [Fact]
        public async void Null_Inventory_List_Throws_Error()
        {
            var service = new InventoryService();
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await service.ReconcileInventoryToAccounting(null, await service.GetAccountingBalances()));
        }

        [Fact]
        public async void Null_Accounting_List_Throws_Error()
        {
            var service = new InventoryService();
            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), null));
        }

        [Fact]
        public async void Empty_Inventory_List_Returns_Accounting_Results()
        {
            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(new List<InventoryBalance>(), await service.GetAccountingBalances());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
        }

        [Fact]
        public async void Empty_Accounting_List_Returns_Inventory_Results()
        {
            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), new List<AccountingBalance>());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
        }

        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            // Create a valid result of our method to test against.
            IEnumerable<InventoryReconciliationResult> test = new List<InventoryReconciliationResult>()
            {
                new InventoryReconciliationResult()
                {
                    ItemNumber = "ABC123",
                    TotalValueOnHandInInventory = 3435M,
                    TotalValueInAccountingBalance = 3435M,
                    InventoryIsBalanced = true
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "ZZZ99",
                    TotalValueOnHandInInventory = 657.53M,
                    TotalValueInAccountingBalance = 1930.62M,
                    InventoryIsBalanced = false
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "zzz99",
                    TotalValueOnHandInInventory = 1273.09M,
                    TotalValueInAccountingBalance = 0.00M,
                    InventoryIsBalanced = false
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "xxccM",
                    TotalValueOnHandInInventory = 7848.00M,
                    TotalValueInAccountingBalance = 7602.75M,
                    InventoryIsBalanced = false
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "xxddM",
                    TotalValueOnHandInInventory = 11212.05M,
                    TotalValueInAccountingBalance = 0.00M,
                    InventoryIsBalanced = false
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "fbr77",
                    TotalValueOnHandInInventory = 0M,
                    TotalValueInAccountingBalance = 17.99M,
                    InventoryIsBalanced = false
                }
            };

            var service = new InventoryService();
            var result = await service.ReconcileInventoryToAccounting(await service.GetInventoryBalances(), await service.GetAccountingBalances());
            Assert.NotEmpty(result);
            Assert.IsType<List<InventoryReconciliationResult>>(result);
            Assert.Equal(6, result.Count());
            Debug.WriteLine(JsonConvert.SerializeObject(result));
            Debug.WriteLine(JsonConvert.SerializeObject(test));
            Assert.True(JsonConvert.SerializeObject(result).Equals(JsonConvert.SerializeObject(test)));
        }
    }
}
