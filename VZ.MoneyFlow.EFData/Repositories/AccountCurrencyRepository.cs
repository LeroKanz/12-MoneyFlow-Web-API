using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;

namespace VZ.MoneyFlow.EFData.Repositories
{
    public class AccountCurrencyRepository : GenericRepository<AccountCurrency>, IAccountCurrencyRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AccountCurrencyRepository(AppDbContext options, IMapper mapper) : base(options, mapper)
        {
            _context = options;
            _mapper = mapper;
        }
    }
}
