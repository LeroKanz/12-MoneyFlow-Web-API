using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;

namespace VZ.MoneyFlow.EFData.Repositories
{
    public class ExchangeRepository : GenericRepository<Exchange>, IExchangeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ExchangeRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Exchange>> GetAllAsync(string userId)
        {
            var exchanges = await _context.Exchanges
                .Where(ex => ex.AccountFrom.UserId == userId 
                    || ex.AccountTo.UserId == userId
                    || ex.AccountFrom.Affiliates.Any(au => au.AffiliateUserId == userId)
                    || ex.AccountTo.Affiliates.Any(au => au.AffiliateUserId == userId))
                .Include(ex => ex.AccountFrom).Include(ex => ex.AccountTo).AsNoTracking().ToListAsync();
            return exchanges;
        }
        public async Task<Exchange> GetByIdAsync(int id, string userId)
        {
            var exchange = await _context.Exchanges
                .Include(t => t.AccountFrom).Include(t => t.AccountTo)
                .FirstOrDefaultAsync(ex => ex.Id == id && (ex.AccountFrom.UserId == userId
                    || ex.AccountTo.UserId == userId
                    || ex.AccountFrom.Affiliates.Any(au => au.AffiliateUserId == userId)
                    || ex.AccountTo.Affiliates.Any(au => au.AffiliateUserId == userId)));
            return exchange;
        }

        public new async Task AddAsync(Exchange exchange)
        {
            exchange.DateTime = DateTime.Now;

            await _context.Exchanges.AddAsync(exchange);
        }
    }
}
