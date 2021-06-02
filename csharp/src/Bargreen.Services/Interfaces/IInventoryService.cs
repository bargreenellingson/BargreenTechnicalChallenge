using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Bargreen.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryBalance>> GetInventoryBalances();

        Task<IEnumerable<AccountingBalance>> GetAccountingBalances();

        Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);
    }
}
