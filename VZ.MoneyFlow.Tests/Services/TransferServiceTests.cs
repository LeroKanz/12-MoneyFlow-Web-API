using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Services.Services;
using VZ.MoneyFlow.Tests.Extensions;

namespace VZ.MoneyFlow.Tests.Services
{
    public class TransferServiceTests : BaseTest
    {
        private readonly TransferService _transferService;
        private readonly Mock<ITransferRepository> _transferRepositoryMock = new();
        private readonly Mock<IAccountCurrencyRepository> _accountCurrencyRepositoryMock = new();

        public TransferServiceTests()
        {
            _transferService = new TransferService(_transferRepositoryMock.Object, _mapper,
                _accountCurrencyRepositoryMock.Object);
        }

        [Test]
        public async Task AddTransferAsync_ShouldUpdateCurrenctFromAndTo()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var transferDto = new TransferDto
            {                
                CurrencyId = 1,
                AccountFromId = 1,
                AccountToId = 2,
                Amount = 20,
            };

            var listOfAccountCurrencies = new List<AccountCurrency>
            {
                new AccountCurrency{ AccountId = 1, CurrencyId = 1, Amount = 100, Account = new Account{ UserId = userId } },
                new AccountCurrency{ AccountId = 2, CurrencyId = 1, Amount = 100, Account = new Account{ UserId = userId } },
                new AccountCurrency{ AccountId = 3, CurrencyId = 3, Amount = 100, Account = new Account{ UserId = userId } }
            };

            _accountCurrencyRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountCurrency, bool>>>()))
                .Returns<Expression<Func<AccountCurrency, bool>>>(filter =>
                  Task.FromResult(listOfAccountCurrencies.FirstOrDefault(l => filter.Compile()(l))));

            _accountCurrencyRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<AccountCurrency>()));

            //Act
            await _transferService.AddAsync(transferDto, userId);
            //Assert
            Assert.That(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == transferDto.AccountFromId)
                .Amount, Is.EqualTo(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == transferDto.AccountFromId).Amount));
            Assert.That(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == transferDto.AccountToId)
                .Amount, Is.EqualTo(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == transferDto.AccountToId).Amount));
        }
    }
}
