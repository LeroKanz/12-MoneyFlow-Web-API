using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeRepository _exchangeRepository;
        private readonly IAccountCurrencyRepository _accountCurrencyRepository;
        private readonly IMapper _mapper;

        public ExchangeService(IExchangeRepository exchangeRepository, IMapper mapper, 
            IAccountCurrencyRepository accountCurrencyRepository)
        {
            _exchangeRepository = exchangeRepository;
            _accountCurrencyRepository = accountCurrencyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExchangeDto>> GetAllAsync(string userId)
        {
            var exchanges = await _exchangeRepository.GetAllAsync(userId);
            var exchangeDtos = _mapper.Map<List<ExchangeDto>>(exchanges);
            return exchangeDtos;
        }

        public async Task<ExchangeDto> GetByIdAsync(int id, string userId)
        {
            var exchange = await _exchangeRepository.GetByIdAsync(id, userId);
            var exchangeDto = _mapper.Map<ExchangeDto>(exchange);
            return exchangeDto;
        }

        public async Task AddAsync(ExchangeDto exchangeDto, string userId)
        {
            var exchange = _mapper.Map<Exchange>(exchangeDto);

            var accountCurrencyFrom = await _accountCurrencyRepository
                    .FirstOrDefaultAsync(ac => 
                    ac.AccountId == exchange.AccountFromId && ac.CurrencyId == exchange.CurrencyFromId
                    && ac.Account.UserId == userId);
            var accountCurrencyTo = await _accountCurrencyRepository
                    .FirstOrDefaultAsync(ac => 
                    ac.AccountId == exchange.AccountToId && ac.CurrencyId == exchange.CurrencyToId
                    && ac.Account.UserId == userId);

            accountCurrencyFrom.Amount -= exchange.AmountFrom;
            accountCurrencyTo.Amount += exchange.AmountTo;

            await _accountCurrencyRepository.UpdateAsync(accountCurrencyFrom);
            await _accountCurrencyRepository.UpdateAsync(accountCurrencyTo);
            exchange.DateTime = System.DateTime.UtcNow;
            await _exchangeRepository.AddAsync(exchange);
            await _exchangeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId )
        {
            var exchange = await _exchangeRepository.GetByIdAsync(id, userId);

            var accountCurrencyFrom = await _accountCurrencyRepository
                    .FirstOrDefaultAsync(ac => ac.AccountId == exchange.AccountFromId && ac.CurrencyId == exchange.CurrencyFromId
                    && ac.Account.UserId == userId);
            var accountCurrencyTo = await _accountCurrencyRepository
                    .FirstOrDefaultAsync(ac => ac.AccountId == exchange.AccountToId && ac.CurrencyId == exchange.CurrencyToId
                    && ac.Account.UserId == userId);

            accountCurrencyFrom.Amount += exchange.AmountFrom;
            accountCurrencyTo.Amount -= exchange.AmountTo;

            await _accountCurrencyRepository.UpdateAsync(accountCurrencyFrom);
            await _accountCurrencyRepository.UpdateAsync(accountCurrencyTo);

            await _exchangeRepository.DeleteAsync(id);
            await _exchangeRepository.SaveChangesAsync();
        }
    }
}
