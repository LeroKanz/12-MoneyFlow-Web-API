using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IExchangeService
    {
        Task<IEnumerable<ExchangeDto>> GetAllAsync(string userId);
        Task<ExchangeDto> GetByIdAsync(int id, string userId);
        Task AddAsync(ExchangeDto exchangeDto, string userId);
        Task DeleteAsync(int id, string userId);
    }
}
