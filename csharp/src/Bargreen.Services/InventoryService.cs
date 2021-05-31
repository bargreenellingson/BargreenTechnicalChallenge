using Bargreen.Services.Dtos;
using Bargreen.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bargreen.Services
{
    public class InventoryService : IInventoryService
    {
        public IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences
            throw new NotImplementedException();
        }
    }
}