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
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Tests.Extensions;

namespace VZ.MoneyFlow.Tests.Controllers
{
    [TestFixture]
    public class TransfersControllerTests : BaseControllerTest
    {
        private readonly TransfersController _transfersController;
        private readonly Mock<ITransferService> _transferService = new();

        public TransfersControllerTests()
        {
            _transfersController = new TransfersController(_transferService.Object, null,
                _mapper);
        }

        [Test]
        public async Task GetAllTransfers_WhenCallde_ReturnListOfTransfers()
        {
            //Arrange
            var listOfTransferDtos = new List<TransferDto>()
            {
                new TransferDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 10 },
                new TransferDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 20 },
                new TransferDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = UserId }, Amount = 30 },
                new TransferDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 40 },
            };
            _transferService.Setup(x => x.GetAllAsync(UserId))
                .ReturnsAsync(listOfTransferDtos.FindAll(ex => ex.AccountFrom.UserId == UserId || ex.AccountTo.UserId == UserId));
            _transfersController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _transfersController.GetAll();
            var okResult = result as OkObjectResult;
            var resultListOfTransferDtos = okResult.Value as List<TransferDto>;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(resultListOfTransferDtos, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetTransferById_WhenCallde_ReturnTransfer()
        {
            //Arrange
            var id = 3;
            var listOfTransferDtos = new List<TransferDto>()
            {
                new TransferDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 10 },
                new TransferDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 20 },
                new TransferDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = UserId }, Amount = 30 },
                new TransferDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 40 },
            };
            _transferService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfTransferDtos.FirstOrDefault(ac => ac.Id == id && (ac.AccountFrom.UserId == UserId || ac.AccountTo.UserId == UserId)));
            _transfersController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            //Act
            var result = await _transfersController.GetById(id);
            var okResult = result as OkObjectResult;
            var transferDto = okResult.Value as TransferDto;
            //Assert
            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(okResult.GetType()));
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(transferDto.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task CreateTransfer_WhenCalled_ReturnOkObjectResult()
        {
            var requestTransferDto = new RequestTransferDto
            { Amount = 20, CurrencyId = 1, AccountFromId = 1, AccountToId = 2 };

            _transferService.Setup(x => x.AddAsync(It.IsAny<TransferDto>(), UserId)).Returns<TransferDto, string>((ac, u) => Task.FromResult(ac));
            _transfersController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _transfersController.Create(requestTransferDto);
            var okResult = result as OkObjectResult;
            var transferDto = okResult.Value as TransferDto;
            
            Assert.That(okResult, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));
            Assert.That(transferDto.AccountFromId == 1, Is.True);
            Assert.That(transferDto.CurrencyId == 2, Is.False);
            Assert.That(transferDto.Amount == 20, Is.True);            
        }

        [Test]
        public async Task DeleteTransfer_WhenCalled_ReturnOkObjectResultAndDeleteObjectFromTheList()
        {
            var id = 1;
            var listOfTransferDtos = new List<TransferDto>()
            {
                new TransferDto { Id = 1, AccountFrom = new AccountOperationDto { UserId = UserId }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 10 },
                new TransferDto { Id = 2, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 20 },
                new TransferDto { Id = 3, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = UserId }, Amount = 30 },
                new TransferDto { Id = 4, AccountFrom = new AccountOperationDto { UserId = "not a user" }, AccountTo = new AccountOperationDto { UserId = "not a user" }, Amount = 40 },
            };
            _transferService.Setup(x => x.GetByIdAsync(id, UserId))
                .ReturnsAsync(listOfTransferDtos.FirstOrDefault(ac => ac.Id == id && (ac.AccountFrom.UserId == UserId || ac.AccountTo.UserId == UserId)));
            _transferService.Setup(x => x.DeleteAsync(id, UserId)).Returns(Task.FromResult(listOfTransferDtos
                .Remove(listOfTransferDtos.FirstOrDefault(x => x.Id == id))));
            _transfersController.ControllerContext = new ControllerContext() { HttpContext = _httpContext.Object };
            var result = await _transfersController.Delete(id);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(typeof(OkObjectResult), Is.EqualTo(result.GetType()));            
            Assert.That(listOfTransferDtos.Count == 3);
            Assert.That(listOfTransferDtos.Contains(listOfTransferDtos.FirstOrDefault(x => x.Id == id)), Is.False);
        }
    }
}
