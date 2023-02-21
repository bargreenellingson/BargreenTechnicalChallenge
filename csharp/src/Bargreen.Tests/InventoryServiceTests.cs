using System;
using System.Collections.Generic;
using System.Linq;
using Bargreen.Services;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        private InventoryService service = new InventoryService();
        [Fact]
        public static void Inventory_Reconciliation_Builds_List()
        {
            var testAccountsList = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                    ItemNumber = "FOO",
                    TotalInventoryValue = 100.0M
                },
                new AccountingBalance()
                {
                    ItemNumber = "BAR",
                    TotalInventoryValue = 100.0M
                }
            };

            var testInventoryList = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "FOO",
                    PricePerItem = 10.0M,
                    QuantityOnHand = 10,
                    WarehouseLocation = "kil"
                },
                new InventoryBalance()
                {
                    ItemNumber = "BAR",
                    PricePerItem = 20.0M,
                    QuantityOnHand = 5,
                    WarehouseLocation = "roy"
                }
            };
            var expectedResult = new List<InventoryReconciliationResult>()
            {
                new InventoryReconciliationResult()
                {
                    ItemNumber = "FOO",
                    TotalValueOnHandInInventory = 100.0M,
                    TotalValueInAccountingBalance = 100.0M,
                    InventoryAccountingDifference = 0.0M
                },
                new InventoryReconciliationResult()
                {
                    ItemNumber = "BAR",
                    TotalValueOnHandInInventory = 100.0M,
                    TotalValueInAccountingBalance = 100.0M,
                    InventoryAccountingDifference = 0.0M
                }
            };

            var actualResult = InventoryService.ReconcileInventoryToAccounting(testInventoryList, testAccountsList).Result.ToList();
            
            Assert.Equal(expectedResult[0].ItemNumber, actualResult[0].ItemNumber);
            Assert.Equal(expectedResult[0].TotalValueOnHandInInventory, actualResult[0].TotalValueOnHandInInventory);
            Assert.Equal(expectedResult[0].TotalValueInAccountingBalance, actualResult[0].TotalValueInAccountingBalance);
            Assert.Equal(expectedResult[0].InventoryAccountingDifference, actualResult[0].InventoryAccountingDifference);
            Assert.Equal(expectedResult[1].ItemNumber, actualResult[1].ItemNumber);
            Assert.Equal(expectedResult[1].TotalValueOnHandInInventory, actualResult[1].TotalValueOnHandInInventory);
            Assert.Equal(expectedResult[1].TotalValueInAccountingBalance, actualResult[1].TotalValueInAccountingBalance);
            Assert.Equal(expectedResult[1].InventoryAccountingDifference, actualResult[1].InventoryAccountingDifference);
        }
        
        [Fact]
        public static void Inventory_Reconciliation_Handles_Multiple_Of_Same_Inv_Item()
        {
            var testAccountsList = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                    ItemNumber = "FOO",
                    TotalInventoryValue = 150.0M
                }
            };

            var testInventoryList = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "FOO",
                    PricePerItem = 10.0M,
                    QuantityOnHand = 10,
                    WarehouseLocation = "kil"
                },
                new InventoryBalance()
                {
                    ItemNumber = "FOO",
                    PricePerItem = 10.0M,
                    QuantityOnHand = 5,
                    WarehouseLocation = "roy"
                }
            };
            var expectedResult = new List<InventoryReconciliationResult>()
            {
                new InventoryReconciliationResult()
                {
                    ItemNumber = "FOO",
                    TotalValueOnHandInInventory = 150.0M,
                    TotalValueInAccountingBalance = 150.0M,
                    InventoryAccountingDifference = 0.0M
                }
            };

            var actualResult = InventoryService.ReconcileInventoryToAccounting(testInventoryList, testAccountsList).Result.ToList();
            
            Assert.Equal(expectedResult[0].ItemNumber, actualResult[0].ItemNumber);
            Assert.Equal(expectedResult[0].TotalValueOnHandInInventory, actualResult[0].TotalValueOnHandInInventory);
            Assert.Equal(expectedResult[0].TotalValueInAccountingBalance, actualResult[0].TotalValueInAccountingBalance);
            Assert.Equal(expectedResult[0].InventoryAccountingDifference, actualResult[0].InventoryAccountingDifference);
        }
        
        [Fact]
        public static void Inventory_Reconciliation_Handles_Same_Inventory_With_Different_Casing()
        {
            var testAccountsList = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                    ItemNumber = "FOO",
                    TotalInventoryValue = 150.0M
                }
            };

            var testInventoryList = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                    ItemNumber = "FOO",
                    PricePerItem = 10.0M,
                    QuantityOnHand = 10,
                    WarehouseLocation = "kil"
                },
                new InventoryBalance()
                {
                    ItemNumber = "foo",
                    PricePerItem = 10.0M,
                    QuantityOnHand = 5,
                    WarehouseLocation = "roy"
                }
            };
            var expectedResult = new List<InventoryReconciliationResult>()
            {
                new InventoryReconciliationResult()
                {
                    ItemNumber = "FOO",
                    TotalValueOnHandInInventory = 150.0M,
                    TotalValueInAccountingBalance = 150.0M,
                    InventoryAccountingDifference = 0.0M
                }
            };

            var actualResult = InventoryService.ReconcileInventoryToAccounting(testInventoryList, testAccountsList).Result.ToList();
            
            Assert.Equal(expectedResult[0].ItemNumber, actualResult[0].ItemNumber);
            Assert.Equal(expectedResult[0].TotalValueOnHandInInventory, actualResult[0].TotalValueOnHandInInventory);
            Assert.Equal(expectedResult[0].TotalValueInAccountingBalance, actualResult[0].TotalValueInAccountingBalance);
            Assert.Equal(expectedResult[0].InventoryAccountingDifference, actualResult[0].InventoryAccountingDifference);
        }
        
        [Fact]
        public static void Inventory_Reconciliation_Adds_Account_Values_Without_Inventory_Item()
        {
            var testAccountsList = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                    ItemNumber = "BAR",
                    TotalInventoryValue = 50.0M
                }
            };

            var testInventoryList = new List<InventoryBalance>()
            {
                
            };
            var expectedResult = new List<InventoryReconciliationResult>()
            {
                
                new InventoryReconciliationResult()
                {
                    ItemNumber = "BAR",
                    TotalValueOnHandInInventory = 0.0M,
                    TotalValueInAccountingBalance = 50.0M,
                    InventoryAccountingDifference = -50.0M
                }
            };

            var actualResult = InventoryService.ReconcileInventoryToAccounting(testInventoryList, testAccountsList).Result.ToList();
            
            Assert.Equal(expectedResult[0].ItemNumber, actualResult[0].ItemNumber);
            Assert.Equal(expectedResult[0].TotalValueOnHandInInventory, actualResult[0].TotalValueOnHandInInventory);
            Assert.Equal(expectedResult[0].TotalValueInAccountingBalance, actualResult[0].TotalValueInAccountingBalance);
            Assert.Equal(expectedResult[0].InventoryAccountingDifference, actualResult[0].InventoryAccountingDifference);
        }
    }
}
