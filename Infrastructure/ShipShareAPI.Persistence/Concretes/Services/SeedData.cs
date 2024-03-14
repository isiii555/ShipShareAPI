using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;


namespace ShipShareAPI.Persistence.Concretes.Services
{
    public static class SeedData
    {
        public async static void Initialize(this WebApplication app)
        {
            using var container = app.Services.CreateScope();

            var configuration = container.ServiceProvider.GetRequiredService<IConfiguration>();

            var userManager = container.ServiceProvider.GetRequiredService<IUserManager>();

            var dbContext = container.ServiceProvider.GetRequiredService<ShipShareDbContext>();

            var roleManager = container.ServiceProvider.GetRequiredService<IRoleManager>();

            var user = await userManager.FindByEmailAsync("admin@admin.com");
            if (user is null)
            {
                PasswordHashHelper.CreatePassword(configuration["DefaultAdmin:Password"]!, out byte[] salt, out byte[] passwordHash);
                var role = await roleManager.GetRoleByName("admin");
                var role2 = await roleManager.GetRoleByName("user");
                user = new User
                {
                    Username = configuration["DefaultAdmin:Username"]!,
                    Email = configuration["DefaultAdmin:Email"]!,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                };

                user = (await dbContext.Users.AddAsync(user)).Entity;

                var userRole = new RoleUser()
                {
                    UserId = user.Id,
                    RoleId = role!.Id
                };
                var userRole2 = new RoleUser()
                {
                    UserId = user.Id,
                    RoleId = role2!.Id
                };

                await dbContext.RoleUser.AddAsync(userRole);
                await dbContext.RoleUser.AddAsync(userRole2);
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
