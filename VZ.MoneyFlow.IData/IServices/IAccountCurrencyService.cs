using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IAccountCurrencyService
    {
        Task AddAsync(AccountCurrencyDto accountCurrencyDto);
        Task UpdateAsync(AccountCurrencyDto accountCurrency);
        Task DeleteAsync(int id);
    }
}
