using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Exceptions;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountsController : BaseAuthController
    {
        private readonly IAccountService _accountService;
        private readonly IAppUserAccountService _appUserAccountService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, UserManager<AppUser> userManager, IMapper mapper,
            IAppUserAccountService appUserAccountService) : base(userManager)
        {
            _accountService = accountService;
            _appUserAccountService = appUserAccountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var accountDtos = await _accountService.GetAllAsync(userId);
            var responseAccountDto = _mapper.Map<List<ResponseAccountDto>>(accountDtos);

            return Ok(responseAccountDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var accountDto = await _accountService.GetByIdAsync(id, userId);

            if (accountDto == null) throw new NotFoundException(nameof(GetById), id);
            if (accountDto != null)
            {
                var responseAccountDto = _mapper.Map<ResponseAccountDto>(accountDto);
                return Ok(responseAccountDto);
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestAccountDto requestAccountDto)
        {
            var userId = GetUserId();

            var newAccountDto = _mapper.Map<AccountDto>(requestAccountDto);
            newAccountDto.UserId = userId;
            await _accountService.AddAsync(newAccountDto);
            return Ok(_mapper.Map<ResponseAccountDto>(newAccountDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RequestUpdateAccountDto requestUpdateAccountDto)
        {
            var userId = GetUserId();
            var accountDto = await _accountService.GetByIdAsync(id, userId);

            if (accountDto == null) throw new NotFoundException(nameof(GetById), id);
            if (accountDto != null)
            {
                var updatedAccount = _mapper.Map(requestUpdateAccountDto, accountDto);

                await _accountService.UpdateAsync(updatedAccount);
                return Ok(_mapper.Map<ResponseAccountDto>(updatedAccount));
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var accountDto = await _accountService.GetByIdAsync(id, userId);
            if (accountDto == null) throw new NotFoundException(nameof(GetById), id);
            if (accountDto != null)
            {
                await _accountService.DeleteAsync(id);
                return Ok("Deleted successfully");
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPut("Affiliate/{id}")]
        public async Task<IActionResult> Affiliate(int id, RequestAffiliateDto requestAffiliateDto)
        {
            var userId = GetUserId();
            var affiliateUser = await _userManager.FindByEmailAsync(requestAffiliateDto.Email);
            var accountDto = await _accountService.GetByIdAsync(id, userId);

            if (accountDto == null) throw new NotFoundException(nameof(GetById), id);
            if (accountDto != null)
            {
                AppUserAccountDto appUserAccountDto = new()
                {
                    AffiliateUserId = affiliateUser.Id,
                    AffiliateAccountId = accountDto.Id,
                };
                await _appUserAccountService.AddAsync(appUserAccountDto);
                return Ok("Has been affiliated successfully");
            }
            return BadRequest("Provided data is invalid");
        }

        [HttpPut("UnAffiliate/{id}")]
        public async Task<IActionResult> UnAffiliate(int id, RequestAffiliateDto requestAffiliateDto)
        {
            var userId = GetUserId();
            var affiliateUser = await _userManager.FindByEmailAsync(requestAffiliateDto.Email);
            var accountDto = await _accountService.GetByIdAsync(id, userId);

            if (accountDto == null) throw new NotFoundException(nameof(GetById), id);
            if (accountDto != null)
            {
                await _appUserAccountService.DeleteAsync(affiliateUser.Id, accountDto.Id);
                return Ok("Has been unaffiliated successfully");
            }
            return BadRequest("Provided data is invalid");
        }
    }
}