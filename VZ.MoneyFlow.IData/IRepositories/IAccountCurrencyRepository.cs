using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface IAccountCurrencyRepository : IGenericRepository<AccountCurrency>
    {
        Task<AccountCurrency> FirstOrDefaultAsync(Expression<Func<AccountCurrency, bool>> filter);
    }
}
