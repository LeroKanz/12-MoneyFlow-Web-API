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
    public class TransfersController : BaseAuthController
    {
        private readonly ITransferService _transferService;
        private readonly IMapper _mapper;

        public TransfersController(ITransferService transferService, UserManager<AppUser> userManager, 
            IMapper mapper) : base(userManager)
        {
            _transferService = transferService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var transferDtos = await _transferService.GetAllAsync(userId);
            return Ok(transferDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var transferDto = await _transferService.GetByIdAsync(id, userId);

            if (transferDto == null) throw new NotFoundException(nameof(GetById), id);
            if (transferDto != null)
            {
                return Ok(transferDto);
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestTransferDto requestTransferDto)
        {
            var userId = GetUserId();

            var newTransferDto = _mapper.Map<TransferDto>(requestTransferDto);
            
            await _transferService.AddAsync(newTransferDto, userId);
            return Ok(newTransferDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var transfer = await _transferService.GetByIdAsync(id, userId);
            if (transfer == null) throw new NotFoundException(nameof(GetById), id);
            if (transfer != null)
            {
                await _transferService.DeleteAsync(id, userId);
                return Ok("Deleted successfully");
            }
            return BadRequest("Provided data is invalid");
        }
    }
}
