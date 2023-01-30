using Microsoft.AspNetCore.Identity;
using System;
using WebService.API.Models;

namespace WebService.API.Data
{
    public static class seedRoles
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Agent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Guest.ToString()));
        }
    }
}
