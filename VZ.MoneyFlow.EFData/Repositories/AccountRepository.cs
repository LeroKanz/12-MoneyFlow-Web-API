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
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AccountRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> GetAllAsync(string userId)
        {            
            var accounts = await _context.Accounts
                .Where(c => c.UserId == userId || c.Affiliates.Any(a => a.AffiliateUserId == userId))
                .Include(c => c.User).Include(c => c.AccountsCurrencies).Include(c => c.Affiliates)
                .ThenInclude(c => c.AffiliateUser)
                .AsNoTracking().ToListAsync();
            return accounts;
        }
        public async Task<Account> GetByIdAsync(int id, string userId)
        {
            var account = await _context.Accounts.AsNoTracking()
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .Include(c => c.AccountsCurrencies)
                .Include(c => c.Affiliates)
                .FirstOrDefaultAsync(acc => acc.Id == id);
            return account;
        }
    }
}
