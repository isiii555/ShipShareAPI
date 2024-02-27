using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
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
            _dbContext = dbContext;
        }

        public Task<bool> CreateAsync(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
