using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebService.API.Authorization;
using WebService.API.Entity;
using WebService.API.Enums;
using WebService.API.Models.UserModels;

namespace WebService.API.Auth
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SuperAdmin, Admin")]
    public class PermissionsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public PermissionsController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("AddPermission/{roleName}")]
        public async Task<IActionResult> AddPermissions(string id, string permission)
        {
            var user = await _userManager.FindByIdAsync(id);    
            if (user == null) {
                var userRoles = await _userManager.GetRolesAsync(user);
    
            await _roleManager.AddClaimAsync(userRoles, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
            await _roleManager.AddClaimAsync(userRoles, new Claim(CustomClaimTypes.Permission, Permissions.Users.Create));
            }
        }

    }
}
