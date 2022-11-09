using AutoMapper;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class AccountCurrencyService : IAccountCurrencyService
    {
        private readonly IAccountCurrencyRepository _accountCurrencyRepository;
        private readonly IMapper _mapper;
        public AccountCurrencyService(IAccountCurrencyRepository accountCurrencyRepository, IMapper mapper)
        {
            _accountCurrencyRepository = accountCurrencyRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(AccountCurrencyDto accountCurrency)
        {
            var newAccountCurrency = _mapper.Map<AccountCurrency>(accountCurrency);
            await _accountCurrencyRepository.AddAsync(newAccountCurrency);
            await _accountCurrencyRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountCurrencyDto accountCurrency)
        {
            var newAccountCurrency = _mapper.Map<AccountCurrency>(accountCurrency);
            await _accountCurrencyRepository.UpdateAsync(newAccountCurrency);
            await _accountCurrencyRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _accountCurrencyRepository.DeleteAsync(id);
            await _accountCurrencyRepository.SaveChangesAsync();
        }
    }
}
