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
        public RoleManageController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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

    }
}
