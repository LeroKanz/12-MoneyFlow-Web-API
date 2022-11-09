using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;

namespace VZ.MoneyFlow.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllWithChildrenAsync(string id)
        {
            var categories = await _categoryRepository.GetAllWithChildrenAsync(id);
            return categories;

        }

        public async Task<Category> GetByIdWithChildrenAsync(int id, string userId)
        {
            var category = await _categoryRepository.GetByIdWithChildrenAsync(id, userId);
            return category;
        }


        public async Task AddAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
