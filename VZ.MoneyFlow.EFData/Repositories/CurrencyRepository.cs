using AutoMapper;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;

namespace VZ.MoneyFlow.EFData.Repositories
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CurrencyRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }
        public async Task<Currency> GetByTypeAsync(int currencyId)
        {
            var currency = await _context.Currencies.FindAsync(currencyId);
            return currency;
        }
    }
}
