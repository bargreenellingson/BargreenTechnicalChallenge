using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargreen.Services
{
    class InventoryComparer : IEqualityComparer<InventoryReconciliationResult>
    {
        public bool Equals(InventoryReconciliationResult a, InventoryReconciliationResult b)
        {
            if (a == null && b == null) { return true; }
            if (a == null | b == null) { return false; }
            if (a.ItemNumber == b.ItemNumber && a.TotalValueInAccountingBalance == b.TotalValueInAccountingBalance && a.TotalValueOnHandInInventory == b.TotalValueOnHandInInventory) { return true; }
            return false;
        }
        public int GetHashCode(InventoryReconciliationResult i)
        {
            string code = i.ItemNumber + "," + i.TotalValueInAccountingBalance + "," + i.TotalValueOnHandInInventory;
            return code.GetHashCode();
        }
    }

    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }

        public Boolean equals(InventoryReconciliationResult b) {
            if (this.ItemNumber == b.ItemNumber && this.TotalValueInAccountingBalance == b.TotalValueInAccountingBalance && this.TotalValueOnHandInInventory == b.TotalValueOnHandInInventory)
            {
                return true;
            }
            return false;
        }
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
        private IEnumerable<InventoryBalance> _inventoryBalances = new List<InventoryBalance>();
        private IEnumerable<AccountingBalance> _accountingBalances = new List<AccountingBalance>();

        public InventoryService()
        {
            InitializeInventoryBalance();
            InitializeAccountingBalance();
        }
        public InventoryService(List<InventoryBalance> invBalances)
        {
            InitializeInventoryBalance(invBalances);
            InitializeAccountingBalance(new List<AccountingBalance>());
        }
        public InventoryService(List<AccountingBalance> accBalance)
        {
            InitializeInventoryBalance(new List<InventoryBalance>());
            InitializeAccountingBalance(accBalance);
        }
        public InventoryService(List<InventoryBalance> invBalances, List<AccountingBalance> accBalance)
        {
            InitializeInventoryBalance(invBalances);
            InitializeAccountingBalance(accBalance);
        }

        // default intitialization of inventory balance if no inventory data is provided
        public void InitializeInventoryBalance()
        {
            _inventoryBalances = new List<InventoryBalance>()
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

        // default intitialization of account balance if no accounting data is provided
        public void InitializeAccountingBalance()
        {
            _accountingBalances = new List<AccountingBalance>()
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

        public void InitializeInventoryBalance(List<InventoryBalance> invBalances) 
        {
            this._inventoryBalances = invBalances;
        }

        public void InitializeAccountingBalance(List<AccountingBalance> accBalance)
        {
            this._accountingBalances = accBalance;
        }

        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            return _inventoryBalances;
        }

        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            return _accountingBalances;
        }

        /**
         * 1: add all values from inventoryBalances into a dictionary where we can map the itemNumber to the totalValue
         * 2: do the same with accountingBalances into a different dictionary
         * 3. if for any 2 items there is a mismatch, create an InventoryReconciliationResult and add it into the result
         * 
         * **/
        public IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            Dictionary<string, decimal> inventoryValues = new Dictionary<string, decimal>();
            Dictionary<string, decimal> accountingValues = new Dictionary<string, decimal>();
            HashSet<InventoryReconciliationResult> result = new HashSet<InventoryReconciliationResult>(new InventoryComparer());

            // fill inventoryValues
            foreach(InventoryBalance item in inventoryBalances)
            {
                var itemNumber = item.ItemNumber;
                var pricePerItem = item.PricePerItem;
                var quantity = item.QuantityOnHand;
                var totalItemVal = pricePerItem * quantity;
                if(!inventoryValues.ContainsKey(itemNumber))
                {
                    inventoryValues.Add(itemNumber, totalItemVal);
                }
                else
                {
                    inventoryValues[itemNumber] += totalItemVal;
                }
            }

            // fill accountingValues
            foreach (AccountingBalance item in accountingBalances)
            {
                var itemNumber = item.ItemNumber;
                var totalInvValue = item.TotalInventoryValue;
                accountingValues.Add(itemNumber, totalInvValue);
            }

            // compare every value in inventory with every value in account
            foreach(KeyValuePair<string, decimal> item in inventoryValues) 
            {
                string itemNumber = item.Key;
                decimal val = item.Value;

                if (!accountingValues.ContainsKey(itemNumber))
                {
                    InventoryReconciliationResult reconsiliationItem = new InventoryReconciliationResult();
                    reconsiliationItem.ItemNumber = itemNumber;
                    reconsiliationItem.TotalValueOnHandInInventory = val;
                    reconsiliationItem.TotalValueInAccountingBalance = 0;
                    result.Add(reconsiliationItem);
                }
                else if(accountingValues[itemNumber] != val)
                {
                    InventoryReconciliationResult reconsiliationItem = new InventoryReconciliationResult();
                    reconsiliationItem.ItemNumber = itemNumber;
                    reconsiliationItem.TotalValueOnHandInInventory = val;
                    reconsiliationItem.TotalValueInAccountingBalance = accountingValues[itemNumber];
                    result.Add(reconsiliationItem);
                }
            }

            // compare every value in account with every value in inventory
            foreach (KeyValuePair<string, decimal> item in accountingValues)
            {
                string itemNumber = item.Key;
                decimal val = item.Value;

                if (!inventoryValues.ContainsKey(itemNumber))
                {
                    InventoryReconciliationResult reconsiliationItem = new InventoryReconciliationResult();
                    reconsiliationItem.ItemNumber = itemNumber;
                    reconsiliationItem.TotalValueOnHandInInventory = 0;
                    reconsiliationItem.TotalValueInAccountingBalance = val;
                    result.Add(reconsiliationItem);
                }
                else if (inventoryValues[itemNumber] != val)
                {
                    InventoryReconciliationResult reconsiliationItem = new InventoryReconciliationResult();
                    reconsiliationItem.ItemNumber = itemNumber;
                    reconsiliationItem.TotalValueOnHandInInventory = inventoryValues[itemNumber];
                    reconsiliationItem.TotalValueInAccountingBalance = val;
                    result.Add(reconsiliationItem);
                }
            }

            return result;
        }
    }
}