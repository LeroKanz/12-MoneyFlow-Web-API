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
    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OperationRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Operation>> GetAllAsync(string userId)
        {
            var accounts = await _context.Operations
                .Where(o => o.Account.UserId == userId || o.Account.Affiliates.Any(au => au.AffiliateUserId == userId))
                .Include(o => o.Category).Include(o => o.Currency).Include(o => o.Account)
                .AsNoTracking().ToListAsync();
            return accounts;
        }

        public async Task<Operation> GetByIdAsync(int id, string userId)
        {
            var opreation = await _context.Operations
                .Include(o => o.Category).Include(o => o.Currency).Include(o => o.Account)
                .FirstOrDefaultAsync(o => o.Id == id && (o.Account.UserId == userId || o.Account.Affiliates.Any(au => au.AffiliateUserId == userId)));
            return opreation;
        }       
    }
}
