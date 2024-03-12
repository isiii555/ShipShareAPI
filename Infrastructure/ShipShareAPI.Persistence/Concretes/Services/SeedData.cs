using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Services
{
    public static class SeedData
    {
        public async static void Initialize(this WebApplication app)
        {
            var container = app.Services.CreateScope();

            var userManager = container.ServiceProvider.GetRequiredService<IUserManager>();

            var dbContext = container.ServiceProvider.GetRequiredService<ShipShareDbContext>();

            var roleManager = container.ServiceProvider.GetRequiredService<IRoleManager>();

            var user = await userManager.FindByEmailAsync("admin@admin.com");
            if (user is null)
            {
                PasswordHashHelper.CreatePassword("admin", out byte[] salt, out byte[] passwordHash);
                var role = await roleManager.GetRoleByName("admin");
                user = new User
                {
                    Username = "Admin",
                    Email = "admin@admin.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                };

                user = (await dbContext.Users.AddAsync(user)).Entity;

                var userRole = new RoleUser()
                {
                    UserId = user.Id,
                    RoleId = role!.Id
                };

                await dbContext.RoleUser.AddAsync(userRole);
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
