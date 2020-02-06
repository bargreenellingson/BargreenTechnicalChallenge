using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System.Text;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }
        /* Added to easily determine both balanced and unbalanced entries
        (saves on doing the comparison later.) */
        public Boolean inventoryIsBalanced { get; set; } 
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


    public class InventoryService: IInventoryService
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

        /* Notes:
            I had considered writing out async helper methods so each of the two lists could be operated on independently.  I eventually decided
            against it as that would require extra runs through each list to consolidate them, and I was trying to keep this method as fast as possible.
            What makes this O(n+m) is that only one pass through each list needs to be performed, but only if done synchronously.  (the calls to Count()
            don't count (lol) as it most likely returns a private count variable instead of iterating through the list)
            Finally, it seemed to me that in an API, something is better than nothing, so returning a result if one or more lists were empty was a
            better option than throwing errors.  Without knowing the greater scope of the program, it's a bit of a tossup as to what the better
            practice would be.*/

        public async Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            /* We can't do nested loops, as that runs in O(n*m), which is not ideal.  Instead, we create a dictionary
             using the inventory enumerable and run a loop over the accounting enumerable to check against it.*/
            var lookup = new Dictionary<string, InventoryReconciliationResult>();
            IEnumerable<InventoryReconciliationResult> result;
            InventoryReconciliationResult temp; // Should this be kept local to the loop to avoid unpredictiable behavior?

            return await Task.Run(() => {
                //Only proceed if both lists are populated.
                if (inventoryBalances.Count() > 0 && accountingBalances.Count() > 0)
                {
                    // If we have entries, iterate over the inventory balances list to create a dictionary. (O(n))
                    foreach (var item in inventoryBalances)
                    {
                        if (lookup.TryGetValue(item.ItemNumber, out temp))
                        {
                            temp.TotalValueOnHandInInventory += item.PricePerItem * item.QuantityOnHand;
                        }
                        else
                        {
                            lookup.Add(item.ItemNumber, new InventoryReconciliationResult()
                            {
                                ItemNumber = item.ItemNumber,
                                TotalValueOnHandInInventory = item.PricePerItem * item.QuantityOnHand,
                                TotalValueInAccountingBalance = 0.0M,
                                inventoryIsBalanced = false
                            });
                        }
                    }

                    // while iterating over accounting, add or update entries in the directory. (O(m))
                    foreach (var item in accountingBalances)
                    {
                        // If a match is found, update entry and decide if it is balanced.
                        if (lookup.TryGetValue(item.ItemNumber, out temp))
                        {
                            // First, update TotalValueInAccountingBalance and then set the boolean.
                            temp.TotalValueInAccountingBalance = item.TotalInventoryValue;
                            temp.inventoryIsBalanced = temp.TotalValueOnHandInInventory == item.TotalInventoryValue;
                        }
                        else
                        {
                            // Create an new entry in the directory since there is an entry in accounting that is not in inventory.
                            lookup.Add(item.ItemNumber, new InventoryReconciliationResult()
                            {
                                ItemNumber = item.ItemNumber,
                                TotalValueOnHandInInventory = 0.0M,
                                TotalValueInAccountingBalance = item.TotalInventoryValue,
                                inventoryIsBalanced = false
                            });
                        }
                    }
                    // return the values of the directory for a run time of O(n+m).
                    result = lookup.Values.ToList<InventoryReconciliationResult>();
                }
                else
                // attempt to return something in the case of an empty list parameter, in case there still results in the other list.
                {
                    // If inventory has entries, add them to the results dictionary.
                    if (inventoryBalances.Count() > 0)
                    {
                        foreach (var item in inventoryBalances)
                        {
                            if (lookup.TryGetValue(item.ItemNumber, out temp))
                            {
                                temp.TotalValueOnHandInInventory += item.PricePerItem * item.QuantityOnHand;
                            }
                            else
                            {
                                lookup.Add(item.ItemNumber, new InventoryReconciliationResult()
                                {
                                    ItemNumber = item.ItemNumber,
                                    TotalValueOnHandInInventory = item.PricePerItem * item.QuantityOnHand,
                                    TotalValueInAccountingBalance = 0.0M,
                                    inventoryIsBalanced = false
                                });
                            }
                        }
                    }

                    // If accounting has entries, add them to the results dictionary.
                    if (accountingBalances.Count() > 0)
                    {
                        // If not empty, iterate over accounting, add entries to the result dictionary. (O(m))
                        foreach (var item in accountingBalances)
                        {
                            // If a match is found, update entry and decide if it is balanced.
                            if (lookup.TryGetValue(item.ItemNumber, out temp))
                            {
                                // First, update TotalValueInAccountingBalance and then set the boolean.
                                temp.TotalValueInAccountingBalance = item.TotalInventoryValue;
                                temp.inventoryIsBalanced = temp.TotalValueOnHandInInventory == item.TotalInventoryValue;
                            }
                            else
                            {
                                // Create an new entry in the directory since there is an entry in accounting that is not in inventory.
                                lookup.Add(item.ItemNumber, new InventoryReconciliationResult()
                                {
                                    ItemNumber = item.ItemNumber,
                                    TotalValueOnHandInInventory = 0.0M,
                                    TotalValueInAccountingBalance = item.TotalInventoryValue,
                                    inventoryIsBalanced = false
                                });
                            }
                        }
                    }

                    result = lookup.Values.ToList<InventoryReconciliationResult>();
                }

                // We are done, return whatever we have.
                return result;
            });
        }
    }
}