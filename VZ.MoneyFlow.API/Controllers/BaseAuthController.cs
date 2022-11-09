using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseAuthController : ControllerBase
    {
        protected readonly UserManager<AppUser> _userManager;
        public BaseAuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        internal async Task<AppUser> GetUserAsync()
        {
            var userEmail = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userEmail);
            return user;
        }

        internal string GetUserId()
        {
            var userId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
    }
}
