using Bargreen.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bargreen.Services.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<AccountingBalance>> GetAccountingBalances();
        Task<IEnumerable<InventoryBalance>> GetInventoryBalances();
    }
}