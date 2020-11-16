using Application.Enums;
using Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    //CUSTOM:MRA:06.09.2020:Added business role
    public static class DefaultBusinessUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "businessuser",
                Email = "businessUser@gmail.com",
                FirstName = "Mozhart",
                LastName = "Murugan",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                BusinessStatus = "pending" //CUSTOM:MRA:06.09.2020:Added business role
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Business.ToString()); //CUSTOM:MRA:06.09.2020:Added business role

                }

            }

            //After registeration the "BusinessStatus" field should be automatically set to "pending".
            //The admin should be able to change the status of a business user
            //Therefore:
            //new Field: BusinessStatus
            //new Action: AUtomatically setting the stattus
            //new Command: Admin changes status
        }
    }
}
