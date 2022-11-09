using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.IData.IRepositories
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        Task<Currency> GetByTypeAsync(int currencyId);
    }
}
