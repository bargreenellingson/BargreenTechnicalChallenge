using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }
    }

    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
    }

    public class AccountingBalance
    {
        public string ItemNumber { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }


    public class InventoryService 
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await Task.Run(() => 
            {
                return new List<InventoryBalance>() 
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
            });
        }


        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await Task.Run(() =>
            {
                return new List<AccountingBalance>()
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
            });
        }

        public async static Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences

            //insntantiate list of InventoryRecResults
            List<InventoryReconciliationResult> inventoryReconciliationResults = new List<InventoryReconciliationResult>();
           
            // iterate through both the inventtory and accounting balances
            foreach(InventoryBalance InvItem in inventoryBalances)
            {
                foreach(AccountingBalance AccNum in accountingBalances)
                {
                    // check that the amounts of items are more than 0
                    if(InvItem.QuantityOnHand > 0 && AccNum.TotalInventoryValue > 0)
                    {
                        //if the items numbers do not match add two reulsts to the list - eahc with their correspoding item number
                        if (InvItem.ItemNumber != AccNum.ItemNumber)
                        {
                            //Invoice item with in invetoryBalances that will be added to list
                            InventoryReconciliationResult inventoryReconciliationResult = new InventoryReconciliationResult()
                            {
                                ItemNumber = InvItem.ItemNumber,
                                TotalValueOnHandInInventory = InvItem.QuantityOnHand,
                                TotalValueInAccountingBalance = AccNum.TotalInventoryValue
                            };
                            inventoryReconciliationResults.Add(inventoryReconciliationResult);

                            //Invoice item in accountingBalances that will be added to list
                            InventoryReconciliationResult _inventoryReconciliationResult = new InventoryReconciliationResult()
                            {
                                ItemNumber = AccNum.ItemNumber,
                                TotalValueOnHandInInventory = InvItem.QuantityOnHand,
                                TotalValueInAccountingBalance = AccNum.TotalInventoryValue
                            };
                            inventoryReconciliationResults.Add(_inventoryReconciliationResult);
                        }
                        // else if the numbers do match add result to the list
                        else
                        {
                            InventoryReconciliationResult inventoryReconciliationResult = new InventoryReconciliationResult()
                            {
                                ItemNumber = InvItem.ItemNumber, // if the item numbers are equal, assigning the number from accounting or inventory will be the same
                                TotalValueOnHandInInventory = InvItem.QuantityOnHand,
                                TotalValueInAccountingBalance = AccNum.TotalInventoryValue
                            };
                            inventoryReconciliationResults.Add(inventoryReconciliationResult);
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    
                }
            }
            // return a list of results 
            return inventoryReconciliationResults;
        }
    }
}