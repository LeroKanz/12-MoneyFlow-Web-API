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
    public class TransferRepository : GenericRepository<Transfer>, ITransferRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TransferRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transfer>> GetAllAsync(string userId)
        {
            var transfers = await _context.Transfers
                .Where(t => t.AccountFrom.UserId == userId 
                    || t.AccountTo.UserId == userId 
                    || t.AccountFrom.Affiliates.Any(au => au.AffiliateUserId == userId) 
                    || t.AccountTo.Affiliates.Any(au => au.AffiliateUserId == userId))
                .Include(t => t.AccountFrom).Include(t => t.AccountTo).AsNoTracking().ToListAsync();
            return transfers;
        }

        public async Task<Transfer> GetByIdAsync(int id, string userId)
        {
            var transfer = await _context.Transfers
                .Include(t => t.AccountFrom).Include(t => t.AccountTo)
                .FirstOrDefaultAsync(t => t.Id == id && (t.AccountFrom.UserId == userId 
                    || t.AccountTo.UserId == userId 
                    || t.AccountFrom.Affiliates.Any(au => au.AffiliateUserId == userId) 
                    || t.AccountTo.Affiliates.Any(au => au.AffiliateUserId == userId)));
            return transfer;
        }
    }
}
