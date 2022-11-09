using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAccountsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public UsersAccountsController(UserManager<AppUser> userManager, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUser([FromBody] RequestUserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var userAlreadyExists = await _userManager.FindByEmailAsync(user.Email);
                if (userAlreadyExists != null)
                {
                    return BadRequest(new ResponseUserRegistrationDto(false, "Email already exists"));                    
                }

                var newUser = new AppUser()
                {
                    UserName = user.FullName,
                    Email = user.Email                    
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                    var jwtToken = await _refreshTokenService.GenerateJwtToken(newUser);
                    return Ok(jwtToken);
                }
                else
                {
                    return Conflict(new ResponseUserRegistrationDto(false, isCreated.Errors.Select(e => e.Description).ToList()));
                }
            }
            else
            {
                return BadRequest(new ResponseUserRegistrationDto(false, "Invalid data"));                    
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RequestUserLoginDto user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = await _userManager.FindByEmailAsync(user.Email);

                if (dbUser == null)
                {
                    return NotFound(new ResponseUserLoginDto(false, "login not found"));                    
                }

                var isCorrect = await _userManager.CheckPasswordAsync(dbUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new ResponseUserLoginDto(false, "Invalid login request"));                    
                }

                var jwtToken = await _refreshTokenService.GenerateJwtToken(dbUser);

                return Ok(jwtToken);
            }

            return BadRequest(new ResponseUserLoginDto(false, "Invalid payload"));            
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RequestTokenDto tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _refreshTokenService.VerifyAndGenerateToken(tokenRequest);
                if (result == null)
                {
                    return BadRequest(new ResponseUserRegistrationDto(false, "Invalid token"));
                }

                return Ok(result);
            }

            return BadRequest(new ResponseUserRegistrationDto(false, "Invalid data"));
        }        
    }
}
