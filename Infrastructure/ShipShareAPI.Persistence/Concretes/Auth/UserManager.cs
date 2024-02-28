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

        public UserManager(ShipShareDbContext dbContext)
        {
            _dbContext = Guard.Against.Null(dbContext);
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            try
            {
                PasswordHashHelper.CreatePassword(password, out byte[] salt, out byte[] passwordHash);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = salt;
                _dbContext.Users.Add(user);
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
    }
}
