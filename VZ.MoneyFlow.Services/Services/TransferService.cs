using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;

namespace VZ.MoneyFlow.Services.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IAccountCurrencyRepository _accountCurrencyRepository;

        private readonly IMapper _mapper;

        public TransferService(ITransferRepository transferRepository, IMapper mapper,
            IAccountCurrencyRepository accountCurrencyRepository)
        {
            _transferRepository = transferRepository;
            _mapper = mapper;
            _accountCurrencyRepository = accountCurrencyRepository;
        }

        public async Task<IEnumerable<TransferDto>> GetAllAsync(string userId)
        {
            var transfers = await _transferRepository.GetAllAsync(userId);
            var transferDtos = _mapper.Map<List<TransferDto>>(transfers);
            return transferDtos;
        }

        public async Task<TransferDto> GetByIdAsync(int id, string userId)
        {
            var transfer = await _transferRepository.GetByIdAsync(id, userId);
            var transferDto = _mapper.Map<TransferDto>(transfer);
            return transferDto;
        }

        public async Task AddAsync(TransferDto transferDto, string userId)
        {
            var transfer = _mapper.Map<Transfer>(transferDto);

            var accountCurrencyFrom = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == transfer.AccountFromId && ac.CurrencyId == transfer.CurrencyId 
                && ac.Account.UserId == userId);
            var accountCurrencyTo = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == transfer.AccountToId && ac.CurrencyId == transfer.CurrencyId
                && ac.Account.UserId == userId);

            accountCurrencyFrom.Amount -= transfer.Amount;
            accountCurrencyTo.Amount += transfer.Amount;

            await _accountCurrencyRepository.UpdateAsync(accountCurrencyFrom);
            await _accountCurrencyRepository.UpdateAsync(accountCurrencyTo);
            transfer.OperationTime = System.DateTime.UtcNow;
            await _transferRepository.AddAsync(transfer);
            await _transferRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var transfer = await _transferRepository.GetByIdAsync(id, userId);
            var accountCurrencyFrom = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == transfer.AccountFrom.Id && ac.CurrencyId == transfer.CurrencyId
                    && ac.Account.UserId == userId);
            var accountCurrencyTo = await _accountCurrencyRepository.
                FirstOrDefaultAsync(ac => ac.AccountId == transfer.AccountTo.Id && ac.CurrencyId == transfer.CurrencyId
                    && ac.Account.UserId == userId);

            accountCurrencyFrom.Amount += transfer.Amount;
            accountCurrencyTo.Amount -= transfer.Amount;

            await _accountCurrencyRepository.UpdateAsync(accountCurrencyFrom);
            await _accountCurrencyRepository.UpdateAsync(accountCurrencyTo);

            await _transferRepository.DeleteAsync(id);
            await _transferRepository.SaveChangesAsync();
        }
    }
}
