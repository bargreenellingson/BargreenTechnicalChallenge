using Bargreen.Services.Dtos;
using System.Collections.Generic;

namespace Bargreen.Services.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);
    }
}