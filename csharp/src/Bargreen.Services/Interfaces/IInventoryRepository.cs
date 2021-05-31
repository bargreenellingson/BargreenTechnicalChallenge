using Bargreen.Services.Dtos;
using System.Collections.Generic;

namespace Bargreen.Services.Interfaces
{
    public interface IInventoryRepository
    {
        IEnumerable<AccountingBalance> GetAccountingBalances();
        IEnumerable<InventoryBalance> GetInventoryBalances();
    }
}