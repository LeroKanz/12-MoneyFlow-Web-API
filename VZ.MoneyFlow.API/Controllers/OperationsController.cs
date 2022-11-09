using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Exceptions;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class OperationsController : BaseAuthController
    {
        private readonly IOperationService _operationService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFormRecognizerService _formRecognizerService;


        public OperationsController(IOperationService operationService, UserManager<AppUser> userManager,
            IMapper mapper, IConfiguration configuration, 
            IFormRecognizerService formRecognizerService) : base(userManager)
        {
            _operationService = operationService;
            _mapper = mapper;
            _configuration = configuration;
            _formRecognizerService = formRecognizerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var operationDtos = await _operationService.GetAllAsync(userId);
            return Ok(operationDtos);
        }

        [HttpGet("GetAllPaged")]
        public async Task<ActionResult<PagedResult<OperationDto>>> GetAllPaged([FromQuery] QueryParameters queryParametrs)
        {
            var userId = GetUserId();
            var operationDtos = await _operationService.GetAllPagedAsync(queryParametrs, userId);
            return Ok(operationDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var operationDto = await _operationService.GetByIdAsync(id, userId);

            if (operationDto == null) throw new NotFoundException(nameof(GetById), id);
            if (operationDto != null)
            {
                return Ok(operationDto);
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestOperationDto requestOperationDto)
        {
            var newOperationDto = _mapper.Map<OperationDto>(requestOperationDto);

            await _operationService.AddAsync(newOperationDto);
            return Ok(newOperationDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var operation = await _operationService.GetByIdAsync(id, userId);
            if (operation == null) throw new NotFoundException(nameof(GetById), id);
            if (operation != null)
            {
                await _operationService.DeleteAsync(id, userId);
                return Ok("Deleted successfully");
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPost("recognize")]
        public async Task<IActionResult> Recognize(IFormFile file)
        {
            var result = await _formRecognizerService.RecognizeFile(file);

            return Ok(result);
        }
    }
}
