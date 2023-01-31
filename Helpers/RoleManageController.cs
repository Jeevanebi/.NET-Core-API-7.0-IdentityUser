using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.API.Models.UserModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.API.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManageController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManageController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // /api/Rolemanager/Roles
        [HttpGet("Roles")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(new UserResponseManager
            {
                IsSuccess= true,
                Message = roles
            });
        }


        // /api/Rolemanager/Roles
        [HttpPost("AddRole/{roleName}")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return Ok(new UserResponseManager
            {
                IsSuccess= true,
                Message= "Role "+roleName+" has been added to Role Manager!"
            });
        }

    }
}
