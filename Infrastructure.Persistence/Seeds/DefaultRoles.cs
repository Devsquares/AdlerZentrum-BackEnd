
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Entities;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            foreach (var role in Enum.GetValues(typeof(RolesEnum)))
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
    }
}
