using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface ITransferService
    {
        Task<IEnumerable<TransferDto>> GetAllAsync(string userId);
        Task<TransferDto> GetByIdAsync(int id, string userId);
        Task AddAsync(TransferDto transferDto, string userId);
        Task DeleteAsync(int id, string userId);
    }
}
