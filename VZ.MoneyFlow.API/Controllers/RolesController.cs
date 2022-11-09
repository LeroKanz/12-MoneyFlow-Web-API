using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            var role = await _roleManager.RoleExistsAsync(name);
            if (!role)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole(name));
                if (newRole.Succeeded)
                {
                    return Ok(new { result = $"The role {name} has been added successfully" });
                }
                else
                {
                    return Conflict(new { error = $"The role {name} has not been created" });
                }
            }
            return BadRequest(new { error = "Role already exists" });
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = "User dose not exist" });
            }
                                    
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
            {
                return NotFound(new { error = "Role dose not exist" });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { result = "Done!" });
            }
            else
            {
                return Conflict(new { error = "Role has not been added to the user" });
            }
        }

        [HttpGet("get-user-roles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = "User dose not exist" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("remove-user-from-role")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = "User dose not exist" });
            }

            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
            {
                return NotFound(new { error = "Role dose not exist" });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { result = "Done!" });
            }
            else
            {
                return Conflict(new { error = "Role has not been removed from the user" });
            }
        }
    }
}
