using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllWithChildrenAsync(string id);
        Task<Category> GetByIdWithChildrenAsync(int id, string userId);

        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
