using AutoMapper;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class AppUserAccountService : IAppUserAccountService
    {
        private readonly IAppUserAccountRepository _appUserAccountRepository;
        private readonly IMapper _mapper;
        public AppUserAccountService(IAppUserAccountRepository appUserAccountRepository, IMapper mapper)
        {
            _appUserAccountRepository = appUserAccountRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(AppUserAccountDto appUserAccountDto)
        {
            var newAppUserAccount = _mapper.Map<AppUserAccount>(appUserAccountDto);
            await _appUserAccountRepository.AddAsync(newAppUserAccount);
            await _appUserAccountRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppUserAccount appUserAccount)
        {
            //var newAccountCurrency = _mapper.Map<AccountCurrency>(accountCurrency);
            await _appUserAccountRepository.UpdateAsync(appUserAccount);
            await _appUserAccountRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(params object[] keys)
        {
            await _appUserAccountRepository.DeleteAsync(keys);
            await _appUserAccountRepository.SaveChangesAsync();
        }
    }
}
