using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAsync(string userId);
        Task<AccountDto> GetByIdAsync(int id, string userId);
        Task AddAsync(AccountDto accountDto);
        Task UpdateAsync(AccountDto account);
        Task DeleteAsync(int id);
    }
}
