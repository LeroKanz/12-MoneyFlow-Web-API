using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface IAppUserAccountRepository : IGenericRepository<AppUserAccount>
    {
        Task<AppUserAccount> FirstOrDefaultAsync(Expression<Func<AppUserAccount, bool>> filter);        
    }
}
