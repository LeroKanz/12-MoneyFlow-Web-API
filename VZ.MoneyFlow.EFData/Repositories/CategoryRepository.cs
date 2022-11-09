using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;

namespace VZ.MoneyFlow.EFData.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllWithChildrenAsync(string id)
        {
            var categories = await _context.Categories.Where(c => c.UserId == id).AsNoTracking().ToListAsync();
            return categories;
        }

        public async Task<Category> GetByIdWithChildrenAsync(int id, string userId)
        {
            var category = await _context.Categories.Where(c => c.UserId == userId).Include(ch => ch.ChildrenCategories)
                .FirstOrDefaultAsync(acc => acc.Id == id);
            return category;
        }
    }
}
