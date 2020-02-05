using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryBalance>> GetInventoryBalances();
        Task<IEnumerable<AccountingBalance>> GetAccountingBalances();
        Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> enumerable1, IEnumerable<AccountingBalance> enumerable2);
    }
}