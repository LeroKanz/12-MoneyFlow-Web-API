using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.API.Controllers;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Tests.Extensions;

namespace VZ.MoneyFlow.Tests.Controllers
{
    [TestFixture]
    public class ExchangesControllerTests : BaseControllerTest
    {
        private readonly ExchangesController _exchangesController;
        private readonly Mock<IExchangeService> _exchangeService = new();

        public ExchangesControllerTests()
        {
            _exchangesController = new ExchangesController(_exchangeService.Object, null,
                _mapper);
        }

        [Test]
        public async Task GetAllExchanges_WhenCallde_ReturnListOfExchanges()
        {
            //Arrange
            var listOfExchangesDtos = new List<ExchangeDto>()
            {
                new ExchangeDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 20, AmountFrom = 10, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 30, AmountFrom = 15, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = UserId }, AmountTo = 40, AmountFrom = 20, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 50, AmountFrom = 25, CurrencyFromId = 1, CurrencyToId = 2 },
            };
            _exchangeService.Setup(x => x.GetAllAsync(UserId))
                .ReturnsAsync(listOfExchangesDtos.FindAll(ex => ex.AccountFrom.UserId == UserId || ex.AccountTo.UserId == UserId));
            _exchangesController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _exchangesController.GetAll();
            var okResult = result as OkObjectResult;
            var listOfExchangeDtos = okResult.Value as List<ExchangeDto>;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(listOfExchangeDtos, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetExchangeById_WhenCallde_ReturnExchange()
        {
            //Arrange
            var id = 3;
            var listOfExchangesDtos = new List<ExchangeDto>()
            {
                new ExchangeDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 20, AmountFrom = 10, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 30, AmountFrom = 15, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = UserId }, AmountTo = 40, AmountFrom = 20, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, AmountTo = 50, AmountFrom = 25, CurrencyFromId = 1, CurrencyToId = 2 },
            };
            _exchangeService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfExchangesDtos.FirstOrDefault(ac => ac.Id == id && (ac.AccountFrom.UserId == UserId || ac.AccountTo.UserId == UserId)));
            _exchangesController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _exchangesController.GetById(id);
            var okResult = result as OkObjectResult;
            var exchangeDto = okResult.Value as ExchangeDto;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(exchangeDto.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task CreateExchange_WhenCalled_ReturnOkObjectResult()
        {
            var requestExchangeDto = new RequestExchangeDto
            { AmountTo = 20, AmountFrom = 10, CurrencyFromId = 1, CurrencyToId = 2, AccountFromId = 1, AccountToId = 2 };
            
            _exchangeService.Setup(x => x.AddAsync(It.IsAny<ExchangeDto>(), UserId)).Returns<ExchangeDto, string>((ac, u) => Task.FromResult(ac));
            _exchangesController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _exchangesController.Create(requestExchangeDto);
            var okResult = result as OkObjectResult;
            var exchangeDto = okResult.Value as ExchangeDto;
            
            Assert.That(okResult, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));
            Assert.That(exchangeDto.AccountFromId == 1, Is.True);
            Assert.That(exchangeDto.CurrencyFromId == 2, Is.False);
            Assert.That(exchangeDto.AmountTo == 20, Is.True);            
        }

        [Test]
        public async Task DeleteExchange_WhenCalled_ReturnOkObjectResultAndDeleteObjectFromTheList()
        {
            var id = 1;
            var listOfExchangesDtos = new List<ExchangeDto>()
            {
                new ExchangeDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AmountTo = 20, AmountFrom = 10, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = UserId }, AmountTo = 30, AmountFrom = 15, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = UserId }, AmountTo = 40, AmountFrom = 20, CurrencyFromId = 1, CurrencyToId = 2 },
                new ExchangeDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AmountTo = 50, AmountFrom = 25, CurrencyFromId = 1, CurrencyToId = 2 },
            };
            _exchangeService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfExchangesDtos.FirstOrDefault(ac => ac.Id == id && (ac.AccountFrom.UserId == UserId || ac.AccountTo.UserId == UserId)));
            _exchangeService.Setup(x => x.DeleteAsync(id, UserId)).Returns(Task.FromResult(listOfExchangesDtos
                .Remove(listOfExchangesDtos.FirstOrDefault(x => x.Id == id))));
            _exchangesController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _exchangesController.Delete(id);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));            
            Assert.That(listOfExchangesDtos.Count == 3);
            Assert.That(listOfExchangesDtos.Contains(listOfExchangesDtos.FirstOrDefault(x => x.Id == id)), Is.False);
        }
    }
}
