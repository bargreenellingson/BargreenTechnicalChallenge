using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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


    public class InventoryService : IInventoryService
    {
        public IEnumerable<InventoryBalance> GetInventoryBalances()
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
                     ItemNumber = "AC123",
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
        }

        public IEnumerable<AccountingBalance> GetAccountingBalances()
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
        }

        public IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences

            //  First we are creating a list to get for each item its total value in hand by multiplying PricePerItem and QuantityOnHand
            List<AccountingBalance> lstInventoryBalance = new List<AccountingBalance>();

            foreach(InventoryBalance invnBalance in inventoryBalances)
            {
                lstInventoryBalance.Add(new AccountingBalance()
                {
                    ItemNumber = invnBalance.ItemNumber,
                    TotalInventoryValue = invnBalance.PricePerItem * invnBalance.QuantityOnHand
                });

            }

            // Create a new lst that will hold the final result of all entries with 
            List<InventoryReconciliationResult> lstInventoryReconciliationResult = new List<InventoryReconciliationResult>();

            //For each item in accountingBalances we will find that if there is a matching entry in new created list in previous step and there value is different 
            //then we will add that entry to list
            //Else we will add  the entry with value 0 as TotalValueOnHandInInventory as this current item is missing in InventoryBalance
            foreach(AccountingBalance acctBalance in accountingBalances)
            {
                var item = lstInventoryBalance.FirstOrDefault(invBal=> invBal.ItemNumber == acctBalance.ItemNumber); 
                if(item != null)
                {
                    if(acctBalance.TotalInventoryValue != item.TotalInventoryValue)
                    {
                    lstInventoryReconciliationResult.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = item.ItemNumber,
                        TotalValueOnHandInInventory = item.TotalInventoryValue,
                        TotalValueInAccountingBalance = acctBalance.TotalInventoryValue
                    });
                    }
                } 
                else 
                {
                    lstInventoryReconciliationResult.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = acctBalance.ItemNumber,
                        TotalValueOnHandInInventory = 0.0M,
                        TotalValueInAccountingBalance = acctBalance.TotalInventoryValue
                    });

                }

            }

            // Now agin we will check for all items of InventoryBalance that are not in resultant lsit and add them in final list.
            foreach(InventoryBalance invBalance in inventoryBalances)
            {
                var item = lstInventoryReconciliationResult.FirstOrDefault(invBal=> invBal.ItemNumber == invBalance.ItemNumber); 
                if(item == null)
                {
                    lstInventoryReconciliationResult.Add(new InventoryReconciliationResult()
                    {
                        ItemNumber = invBalance.ItemNumber,
                        TotalValueOnHandInInventory = lstInventoryBalance.First(acct => acct.ItemNumber == invBalance.ItemNumber).TotalInventoryValue,
                        TotalValueInAccountingBalance = 0.0M
                    });
                } 
                

            }
             return lstInventoryReconciliationResult;

        }
    }
}