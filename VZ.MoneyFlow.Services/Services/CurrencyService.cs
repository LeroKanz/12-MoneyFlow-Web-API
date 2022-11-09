using AutoMapper;
using System.Threading.Tasks;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<CurrencyDto> GetByTypeAsync(int currencyId)
        {
            var currency = await _currencyRepository.GetByTypeAsync(currencyId);
            var currencyDto = _mapper.Map<CurrencyDto>(currency);
            return currencyDto;
        }
    }
}
