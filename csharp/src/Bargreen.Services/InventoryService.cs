using System;
using System.Collections.Generic;
using System.Text;

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

    public class InventoryBalances                                          //this class creates and populates a list of InventoryBalance class
    {
        private List<InventoryBalance> inventoryBalanceList;

        public InventoryBalances()
        {
            inventoryBalanceList = new List<InventoryBalance>()
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
        }

        //This function exposes the populated List of InventoryBalance
        public List<InventoryBalance> getInventoryBalanceList()
        {
            return this.inventoryBalanceList;
        }

        //This function gets the total price of all items in the inventory per item number
        public int getTotalPerItem(string itemNumber)
        {
            decimal total = 0.00M;
            foreach (InventoryBalance item in this.inventoryBalanceList)
            {
                if (item.ItemNumber == itemNumber)
                {
                    decimal product = item.PricePerItem * item.QuantityOnHand;
                    total += product;
                }
            }
            return total;
        }
    }

    public class AccountingBalances
    {
        private List<AccountingBalance> accountingBalanceList;

        public AccountingBalances()
        {
            this.accountingBalanceList = new List<AccountingBalance>()
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

        //This function exposes the populated List of AccountingBalance
        public List<AccountingBalance> getAccountingBalanceList()
        {
            return this.accountingBalanceList;
        }
    }

    public class InventoryService
    {
        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            return InventoryBalances.getInventoryBalanceList();
        }

        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            return AccountingBalances.getAccountingBalanceList();
        }

        public static IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences
            // throw new NotImplementedException();

            try
            {
                AccountingBalances accounting = new AccountingBalances();
                InventoryBalances inventory = new InventoryBalances();

                List<InventoryReconciliationResult> resultList = new List<InventoryReconciliationResult>();
                List<AccountingBalance> accountingBalancesList = accounting.getAccountingBalanceList();
                List<InventoryBalance> inventoryBalancesList = inventory.getInventoryBalanceList();

                foreach (AccountingBalance accountingBalanceItem in accountingBalancesList)
                {
                    InventoryReconciliationResult result = new InventoryReconciliationResult();
                    result.ItemNumber = accountingBalanceItem.ItemNumber;
                    result.TotalValueInAccountingBalance = accountingBalanceItem.TotalInventoryValue;
                    result.TotalValueOnHandInInventory = inventory.getTotalPerItem();
                    resultList.Add(result);
                }

                return resultList;
            }
            catch (System.Exception)
            {
                throw System.Exception;
            }

        }
    }
}