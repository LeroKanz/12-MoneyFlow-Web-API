using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface IExchangeRepository : IGenericRepository<Exchange>
    {
        Task<IEnumerable<Exchange>> GetAllAsync(string userId);
        Task<Exchange> GetByIdAsync(int id, string userId);
    }
}
