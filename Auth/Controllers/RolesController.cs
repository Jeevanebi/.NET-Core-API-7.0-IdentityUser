
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.API.Authorization;
using WebService.API.Models.UserModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.API.Auth.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer"/*, Roles = "SuperAdmin, Admin"*/)]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        // /api/Roles
        //[Authorize(AuthenticationSchemes = "Bearer",/* Policy = "SuperAdmin",*/ Roles = "SuperAdmin,Admin,")]
        [Authorize(Policy = "ViewPolicy")]
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

        //[Authorize(AuthenticationSchemes = "Bearer", /*Policy = "SuperAdmin",*/ Roles = "SuperAdmin")]
 
        [HttpGet("userRoles/{userId}")]
        [Authorize(Permissions.Users.View)]
        public async Task<IActionResult> GetUserRolebyId(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                return Ok(roles);
            };
            return BadRequest("User not found!");

        }

        // /api/Roles/{RoleName}
        //[Authorize(AuthenticationSchemes = "Bearer", /*Policy = "SuperAdmin",*/ Roles = "SuperAdmin")]
        [Authorize(Permissions.Users.Create)]
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
        [Authorize(Permissions.Users.Create)]
        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole(string userId, string userRole)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (userRole != null)
            {
                if (await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.AddToRoleAsync(existingUser, userRole);
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                };
                return BadRequest("Role does not exits!");
            }
            return NotFound("Please fill the required fields ! ");
        }

        [Authorize(Permissions.Users.Create)]
        [HttpPost("RemoveUserRole")]
        public async Task<IActionResult> RemoveUserRole(string userId, string userRole)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (userRole != null)
            {
                if (await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.RemoveFromRoleAsync(existingUser, userRole);
                    var addedRoles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(addedRoles);
                };
                return BadRequest("Role does not exits!");
            }
            return NotFound("Please fill the required fields ! ");
        }


    }
}
