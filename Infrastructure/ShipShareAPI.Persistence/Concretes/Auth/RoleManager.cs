using Ardalis.GuardClauses;
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
    public class RoleManager : IRoleManager
    {
        private readonly ShipShareDbContext _dbContext;

        public RoleManager(ShipShareDbContext dbContext)
        {
            _dbContext = Guard.Against.Null(dbContext);
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRoleByEmail(string email)
        {
            var user = await _dbContext.Users.Include(u => u.Roles).Where(u => u.Email == email).FirstOrDefaultAsync();
            if (user is not null)
                return user!.Roles!;
            throw new Exception("Email not found!(RoleManager)");
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await _dbContext.Roles.Where(r => r.Name == roleName).FirstOrDefaultAsync();
        }
    }
}
