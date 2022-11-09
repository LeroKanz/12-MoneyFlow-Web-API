using AutoMapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Paging;

namespace VZ.MoneyFlow.Services.Services
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IAccountCurrencyRepository _accountCurrencyRepository;
        private readonly IMapper _mapper;

        public OperationService(IOperationRepository operationRepository, IMapper mapper,
            IAccountCurrencyRepository accountCurrencyRepository, IHttpClientFactory httpClientFactory) 
        {
            _accountCurrencyRepository = accountCurrencyRepository;
            _operationRepository = operationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationDto>> GetAllAsync(string userId)
        {
            var operations = await _operationRepository.GetAllAsync(userId);
            var operationDtos = _mapper.Map<List<OperationDto>>(operations);
            return operationDtos;
        }

        public async Task<OperationDto> GetByIdAsync(int id, string userId)
        {
            var operation = await _operationRepository.GetByIdAsync(id, userId);
            var operationDtos = _mapper.Map<OperationDto>(operation);
            return operationDtos;
        }

        public async Task AddAsync(OperationDto operationDto)
        {
            var operation = _mapper.Map<Operation>(operationDto);

            var accountCurrency = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == operation.AccountId && ac.CurrencyId == operation.CurrencyId);

            if (operation.OperationType == OperationType.Income)
                accountCurrency.Amount += operation.Amount;
            if (operation.OperationType == OperationType.Expenses)
                accountCurrency.Amount -= operation.Amount;

            await _accountCurrencyRepository.UpdateAsync(accountCurrency);
            operation.OperationTime = System.DateTime.UtcNow;
            await _operationRepository.AddAsync(operation);
            await _operationRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var operation = await _operationRepository.GetByIdAsync(id, userId);
            var accountCurrency = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == operation.AccountId && ac.CurrencyId == operation.CurrencyId);

            if (operation.OperationType == OperationType.Income)
                accountCurrency.Amount -= operation.Amount;
            if (operation.OperationType == OperationType.Expenses)
                accountCurrency.Amount += operation.Amount;

            await _accountCurrencyRepository.UpdateAsync(accountCurrency);
            await _operationRepository.DeleteAsync(id);
            await _operationRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<OperationDto>> GetAllPagedAsync(QueryParameters queryParametrs, string userId )
        {            
            return await _operationRepository.GetAllPagedAsync<OperationDto>(queryParametrs, o => o.Account.UserId == userId);
        }        
    }
}
