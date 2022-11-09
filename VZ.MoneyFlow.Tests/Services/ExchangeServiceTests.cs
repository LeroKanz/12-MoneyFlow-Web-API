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
    [TestFixture]
    public class ExchangeServiceTests : BaseTest
    {
        private readonly ExchangeService _exchangeService;
        private readonly Mock<IExchangeRepository> _exchangeRepositoryMock = new();
        private readonly Mock<IAccountCurrencyRepository> _accountCurrencyRepositoryMock = new();


        public ExchangeServiceTests()
        {
            _exchangeService = new ExchangeService(_exchangeRepositoryMock.Object, _mapper, 
                _accountCurrencyRepositoryMock.Object);
        }

        [Test]
        public async Task AddExchangeAsync_ShouldUpdateCurrenctFromAndTo()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var exchangeDto = new ExchangeDto
            {
                AmountFrom = 10,
                AmountTo = 20,
                CurrencyFromId = 1,
                CurrencyToId = 2,
                AccountFromId = 1,
                AccountToId = 2
            };
            var listOfAccountCurrencies = Enumerable.Range(1, 3)
                .Select(i => new AccountCurrency { AccountId = i, CurrencyId = i, Amount = 100, Account = new Account { UserId = userId } });
            
            _accountCurrencyRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountCurrency, bool>>>()))
                .Returns<Expression<Func<AccountCurrency, bool>>>(filter => 
                  Task.FromResult(listOfAccountCurrencies.FirstOrDefault(l => filter.Compile()(l))));
            
            _accountCurrencyRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<AccountCurrency>()));            

            //Act
            await _exchangeService.AddAsync(exchangeDto, userId);
            //Assert
            Assert.That(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == exchangeDto.AccountFromId)
                .Amount, Is.EqualTo(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == exchangeDto.AccountFromId).Amount));
            Assert.That(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == exchangeDto.AccountToId)
                .Amount, Is.EqualTo(listOfAccountCurrencies.FirstOrDefault(ac => ac.AccountId == exchangeDto.AccountToId).Amount));
        }
    }
}
