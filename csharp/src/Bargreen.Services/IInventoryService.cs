using System;
using System.Collections.Generic;
using System.Text;

namespace Bargreen.Services
{
    public interface  IInventoryService
    {
        void InitializeInventoryBalance();
        void InitializeAccountingBalance();
        IEnumerable<InventoryBalance> GetInventoryBalances();
        IEnumerable<AccountingBalance> GetAccountingBalances();
        IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);

    }
}
