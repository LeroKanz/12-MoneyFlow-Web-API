using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface IOperationRepository : IGenericRepository<Operation>
    {
        Task<IEnumerable<Operation>> GetAllAsync(string userId);
        Task<Operation> GetByIdAsync(int id, string userId);
    }
}
