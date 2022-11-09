using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync(string userId)
        {
            var accounts = await _accountRepository.GetAllAsync(userId);
            var accountDtos = _mapper.Map<List<AccountDto>>(accounts);
            return accountDtos;
        }

        public async Task<AccountDto> GetByIdAsync(int id, string userId)
        {
            var account = await _accountRepository.GetByIdAsync(id, userId);
            var accountDto = _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task AddAsync(AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();            
        }

        public async Task UpdateAsync(AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            await _accountRepository.UpdateAsync(account);
            await _accountRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
            await _accountRepository.SaveChangesAsync();
        }
    }
}
