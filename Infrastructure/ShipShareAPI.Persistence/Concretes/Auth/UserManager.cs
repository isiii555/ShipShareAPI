using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class UserManager : IUserManager
    {
        private readonly ShipShareDbContext _dbContext;
        private readonly IRoleManager _roleManager;

        public UserManager(ShipShareDbContext dbContext, IRoleManager roleManager)
        {
            _dbContext = Guard.Against.Null(dbContext);
            _roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            try
            {
                PasswordHashHelper.CreatePassword(password, out byte[] salt, out byte[] passwordHash);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = salt;
                _dbContext.Users.Add(user);
                var role = await _roleManager.GetRoleByName("User");
                if (role is not null)
                {
                    var roleUser = new RoleUser()
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                    };
                    _dbContext.RoleUser.Add(roleUser);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithRefreshToken(string refreshToken)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateRefreshToken(User user, string refreshToken, DateTime accessTokenDate)
        {
            if (user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireDate = accessTokenDate.AddMinutes(10);
                _dbContext.Entry(user!.Roles[0]!).State = EntityState.Detached;
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
