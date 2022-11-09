using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface ITransferRepository : IGenericRepository<Transfer>
    {
        Task<IEnumerable<Transfer>> GetAllAsync(string userId);
        Task<Transfer> GetByIdAsync(int id, string userId);
    }
}
