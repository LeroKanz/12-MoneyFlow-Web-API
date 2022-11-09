using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Services.Services;
using VZ.MoneyFlow.Tests.Extensions;

namespace VZ.MoneyFlow.Tests.Services
{
    [TestFixture]
    public class AccountServiceTests : BaseTest
    {
        private readonly AccountService _accountService;
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
        
        public AccountServiceTests()
        {
            _accountService = new AccountService(_accountRepositoryMock.Object, _mapper);                       
        }
        
        [Test]
        public async Task GetAccountAsync_ShouldReturnAccount_IfAccountExists()
        {
            //Arrange
            var accountId = 1;
            var userId = Guid.NewGuid().ToString();
            var account = new Account { Id = accountId, UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetByIdAsync(accountId, userId))
                .Returns(Task.FromResult(account));
            //Act
            var accountDto = await _accountService.GetByIdAsync(accountId, userId);
            //Assert
            Assert.That(accountDto, Is.Not.Null);
            Assert.That(accountDto.Id, Is.EqualTo(account.Id));            
        }
        [Test]
        public async Task GetAllAccountsAsync_ShouldReturnListOfAccount()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var listOfAccounts = new List<Account>()
            {
                new Account { Id = 1, UserId = userId },
                new Account { Id = 2, UserId = userId },
                new Account { Id = 3, UserId = userId },
                new Account { Id = 4, UserId = "not-a-user" },
            };
            _accountRepositoryMock.Setup(x => x.GetAllAsync(userId))
                .ReturnsAsync(listOfAccounts.Where(ac => ac.UserId == userId));            
            //Act
            var listOfAccountDtos = await _accountService.GetAllAsync(userId);
            //Assert
            Assert.That(listOfAccountDtos, Is.Not.Null);
            Assert.That(listOfAccountDtos.Count(), Is.EqualTo(3));
            Assert.That(listOfAccountDtos.All(i => i.UserId == userId), Is.True);
        }

        [Test]
        public async Task CreateAccount_WhenCalled_ReturnOkObjectResult()
        {
            var userId = Guid.NewGuid().ToString();
            var accountDto = new AccountDto
            {
                UserId = userId,
                AccountsCurrencies = new List<AccountCurrencyDto> { new AccountCurrencyDto (100, 1, 1) },
                AccountType = AccountType.CertificateOfDeposit,
                BankAccountNumber = "4444-777",
                LastFourDigitsOfCard = 3333,
                Name = "Account"
            };

            _accountRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Account>()));
            await _accountService.AddAsync(accountDto);

            var isCompletedSafe = _accountRepositoryMock.Object.SaveChangesAsync().IsCompletedSuccessfully;
            var isCompletedAdd = _accountRepositoryMock.Object.AddAsync(It.IsAny<Account>()).IsCompletedSuccessfully;

            _accountRepositoryMock.VerifyAll();
            _accountRepositoryMock.Verify(mock => mock.AddAsync(It.IsAny<Account>()));
            Assert.That(isCompletedSafe, Is.True);
            Assert.That(isCompletedAdd, Is.True);
            Assert.That(_accountService.AddAsync(accountDto).IsCompletedSuccessfully, Is.True);            
        }
    }
}