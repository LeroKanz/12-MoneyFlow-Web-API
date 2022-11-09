using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllWithChildrenAsync(string id);
        Task<Category> GetByIdWithChildrenAsync(int id, string userId);
    }
}
