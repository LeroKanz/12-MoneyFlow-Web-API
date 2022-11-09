using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VZ.MoneyFlow.API.Controllers;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;
using VZ.MoneyFlow.Tests.Extensions;

namespace VZ.MoneyFlow.Tests.Controllers
{
    [TestFixture]
    public class AccountsControllerTests : BaseControllerTest
    {
        private readonly AccountsController _accountController;
        private readonly Mock<IAccountService> _accountService = new();
        private readonly Mock<IAppUserAccountService> _appUserAccountService = new();

        public AccountsControllerTests()
        {
            _accountController = new AccountsController(_accountService.Object, null,
                _mapper, _appUserAccountService.Object);
        }

        [Test]
        public async Task GetAllAccounts_WhenCallde_ReturnListOfAccount()
        {
            //Arrange            
            var listOfAccounts = new List<AccountDto>
            {
                new AccountDto { Id = 1, UserId = UserId },
                new AccountDto { Id = 2, UserId = UserId },
                new AccountDto { Id = 3, UserId = UserId },
                new AccountDto { Id = 4, UserId = "not-a-user" },
            };
            _accountService.Setup(x => x.GetAllAsync(UserId))
                .ReturnsAsync(listOfAccounts.Where(ac => ac.UserId == UserId));
            _accountController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _accountController.GetAll();
            var okResult = result as OkObjectResult;
            var listOfResponseAccountDtos = okResult.Value as List<ResponseAccountDto>;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(listOfResponseAccountDtos, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task GetAccountById_WhenCallde_ReturnAccount()
        {
            //Arrange
            var id = 2;            
            var listOfAccounts = new List<AccountDto>
            {
                new AccountDto { Id = 1, UserId = UserId },
                new AccountDto { Id = 2, UserId = UserId },
                new AccountDto { Id = 3, UserId = UserId },
                new AccountDto { Id = 4, UserId = "not-a-user" },
            };
            _accountService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfAccounts.FirstOrDefault(ac => ac.UserId == UserId && ac.Id == id));
            _accountController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _accountController.GetById(id);
            var okResult = result as OkObjectResult;
            var responseAccountDtos = okResult.Value as ResponseAccountDto;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(responseAccountDtos.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task UpdateAccount_WhenCalled_ReturnOkObjectResult()
        {
            var accountId = 1;
            var requestUpdateAccountDto = new RequestUpdateAccountDto
            {
                AccountType = AccountType.CertificateOfDeposit,
                BankAccountNumber = "4444-777",
                LastFourDigitsOfCard = 3333,
                Name = "Account"
            };
            _accountService.Setup(x => x.GetByIdAsync(accountId, UserId)).ReturnsAsync(new AccountDto());
            _accountService.Setup(x => x.UpdateAsync(It.IsAny<AccountDto>())).Returns<AccountDto>(ac => Task.FromResult(ac));
            _accountController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _accountController.Update(accountId, requestUpdateAccountDto);

            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CreateAccount_WhenCalled_ReturnOkObjectResult()
        {
            var requestAccountDto = new RequestAccountDto
            {
                Amount = 3000,
                CurrencyId = (int)CurrencyType.USD,
                AccountType = AccountType.CertificateOfDeposit,
                BankAccountNumber = "4444-777",
                LastFourDigitsOfCard = 3333,
                Name = "Account"
            };
            _accountService.Setup(x => x.AddAsync(It.IsAny<AccountDto>())).Returns<AccountDto>(ac => Task.FromResult(ac));
            _accountController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _accountController.Create(requestAccountDto);

            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteAccount_WhenCalled_ReturnOkObjectResultAndDeleteObjectFromTheList()
        {
            var id = 1;
            var listOfAccountsDtos = new List<AccountDto>()
            {
                new AccountDto { Id = 1, UserId = UserId },
                new AccountDto { Id = 2, UserId = "not-a-user" },
                new AccountDto { Id = 3, UserId = UserId },
                new AccountDto { Id = 4, UserId = "not-a-user-as-well" },
            };
            _accountService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfAccountsDtos.FirstOrDefault(ac => ac.UserId == UserId && ac.Id == id));
            _accountService.Setup(x => x.DeleteAsync(id)).Returns(Task.FromResult(listOfAccountsDtos
                .Remove(listOfAccountsDtos.FirstOrDefault(x => x.Id == id))));
            _accountController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _accountController.Delete(id);

            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));
            Assert.That(result, Is.Not.Null);
            Assert.That(listOfAccountsDtos.Count == 3);
            Assert.That(listOfAccountsDtos.Contains(listOfAccountsDtos.FirstOrDefault(x => x.Id == id)), Is.False);
        }
    }
}
