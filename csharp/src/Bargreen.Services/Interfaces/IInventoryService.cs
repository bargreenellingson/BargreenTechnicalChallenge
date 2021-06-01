using Bargreen.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bargreen.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);
    }
}