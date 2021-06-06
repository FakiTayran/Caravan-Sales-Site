using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public static class ElevenFeetDbSeeder
    {
        public static async Task SeedRolesAndUsersAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            var roleName = "admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var userName = "omerariduru@gmail.com";
            if (!await userManager.Users.AnyAsync(x => x.UserName == userName))
            {
                var user = new IdentityUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user, "!Rs2232318?");
                await userManager.AddToRoleAsync(user, roleName);
            }
        }

        public static async Task<IHost> SeedAsync(this IHost host)
        {
            // http://www.binaryintellect.net/articles/5e180dfa-4438-45d8-ac78-c7cc11735791.aspx
            // https://github.com/dotnet-architecture/eShopOnWeb/blob/master/src/Web/Startup.cs
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var env = serviceProvider.GetRequiredService<IHostEnvironment>();
                var db = serviceProvider.GetRequiredService<ElevenFeetDbContext>();
                await SeedRolesAndUsersAsync(roleManager, userManager);
            }
            return host;
        }
    }
}
