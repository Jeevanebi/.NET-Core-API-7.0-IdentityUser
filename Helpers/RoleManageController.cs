
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.API.Models.UserModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.API.Helpers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoleManageController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleManageController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

   

        // /api/Roles
        [Authorize(AuthenticationSchemes = "Bearer",/* Policy = "SuperAdmin",*/ Roles ="SuperAdmin,Admin")]
        [HttpGet("Roles")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new UserResponseManager
            {
                IsSuccess = true,
                Message = roles
            });
        }


        // /api/Roles/{RoleName}
        [Authorize(AuthenticationSchemes = "Bearer", /*Policy = "SuperAdmin",*/ Roles = "SuperAdmin")]
        [HttpPost("AddRole/{roleName}")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return Ok(new UserResponseManager
            {
                IsSuccess = true,
                Message = "Role " + roleName + " has been added to Role Manager!"
            });
        }

        // /api/AddUserRole/{RoleName}
        //[Authorize(AuthenticationSchemes = "Bearer", /*Policy = "SuperAdmin",*/ Roles = "SuperAdmin")]
        [AllowAnonymous]
        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole(string id, string roles)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (roles != null)
            {
                if (await _roleManager.RoleExistsAsync(roles)) {
                    await _userManager.AddToRoleAsync(existingUser, roles);
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                };
                return BadRequest("Role does not exits!"); 
            }
            return NotFound("Please fill the required fields ! ");
;
        }

    }
}
