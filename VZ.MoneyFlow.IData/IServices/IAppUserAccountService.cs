using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IAppUserAccountService
    {
        Task AddAsync(AppUserAccountDto appUserAccountDto);
        Task UpdateAsync(AppUserAccount appUserAccount);
        Task DeleteAsync(params object[] keys);
    }
}
