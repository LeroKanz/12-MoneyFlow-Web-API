using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ClaimsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(user);
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            return Ok(userClaims);
        }

        [HttpPost]
        public async Task<IActionResult> AddClaimToUser(string email, string claimName, string claimValue)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(user);
            }

            var userClaim = new Claim(claimName, claimValue);
            var result = await _userManager.AddClaimAsync(user, userClaim);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            
            return Conflict(new { error = "Claim has not been added" });                        
        }
    }
}
