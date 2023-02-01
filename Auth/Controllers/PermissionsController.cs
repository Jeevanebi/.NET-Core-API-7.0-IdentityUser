using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using WebService.API.Authorization;
using WebService.API.Entity;
using WebService.API.Enums;
using WebService.API.Models.UserModels;


namespace WebService.API.Auth.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SuperAdmin")]
    public class PermissionsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public PermissionsController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("AddPermission/{Role}")]
        public async Task<IActionResult> AddPermissions(string Role)
        {

            if (Role != null)
            {
                switch (Role)
                {
                    case "SuperAdmin":
                        var superAdmin = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.Users.Create));
                        await _roleManager.AddClaimAsync(superAdmin, new Claim(CustomClaimTypes.Permission, Permissions.Users.Delete));
                        break;
                    case "Admin":
                        var Admin = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(Admin, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                        await _roleManager.AddClaimAsync(Admin, new Claim(CustomClaimTypes.Permission, Permissions.Users.Create));
                        await _roleManager.AddClaimAsync(Admin, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                        break;
                    case "Agent":
                        var Agent = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(Agent, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                        await _roleManager.AddClaimAsync(Agent, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                        break;
                    case "Client":
                        var Client = await _roleManager.FindByNameAsync(Role);
                        await _roleManager.AddClaimAsync(Client, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                        break;
                    default:
                        var Guest = await _roleManager.FindByNameAsync("guest");
                        await _roleManager.AddClaimAsync(Guest, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                        break;

                }

                var userRole = await _userManager.FindByNameAsync(Role);
                return Ok(userRole);
            }
            return BadRequest("Role not defined!");
        }

    }
}
