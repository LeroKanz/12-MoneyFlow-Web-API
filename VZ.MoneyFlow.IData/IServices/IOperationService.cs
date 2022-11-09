using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Paging;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IOperationService
    {
        Task<IEnumerable<OperationDto>> GetAllAsync(string userId);
        Task<OperationDto> GetByIdAsync(int id, string userId);
        Task AddAsync(OperationDto operationDto);
        Task DeleteAsync(int id, string userId);
        Task<PagedResult<OperationDto>> GetAllPagedAsync(QueryParameters queryParametrs, string userId);
    }
}
