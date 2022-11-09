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

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ExchangesController : BaseAuthController
    {
        private readonly IExchangeService _exchangeService;
        private readonly IMapper _mapper;

        public ExchangesController(IExchangeService exchangeService, UserManager<AppUser> userManager, 
            IMapper mapper) : base(userManager)
        {
            _exchangeService = exchangeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var exchangeDtos = await _exchangeService.GetAllAsync(userId);
            return Ok(exchangeDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var exchangeDto = await _exchangeService.GetByIdAsync(id, userId);

            if (exchangeDto == null) throw new NotFoundException(nameof(GetById), id);
            if (exchangeDto != null)
            {
                return Ok(exchangeDto);
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestExchangeDto requestExchangeDto)
        {
            var userId = GetUserId();
            var newExchangeDto = _mapper.Map<ExchangeDto>(requestExchangeDto);
            
            await _exchangeService.AddAsync(newExchangeDto, userId);
            return Ok(newExchangeDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var exchange = await _exchangeService.GetByIdAsync(id, userId);
            if (exchange == null) throw new NotFoundException(nameof(GetById), id);
            if (exchange != null)
            {
                await _exchangeService.DeleteAsync(id, userId);
                return Ok("Deleted successfully");
            }
            return BadRequest("Provided data is invalid");
        }
    }
}
