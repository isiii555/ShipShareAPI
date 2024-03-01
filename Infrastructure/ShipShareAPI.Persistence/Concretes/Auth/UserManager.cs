using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class UserManager : IUserManager
    {
        private readonly ShipShareDbContext _dbContext;
        private readonly IRoleManager _roleManager;
        private readonly ITokenHandler _tokenHandler;

        public UserManager(ShipShareDbContext dbContext, IRoleManager roleManager, ITokenHandler tokenHandler)
        {
            _dbContext = Guard.Against.Null(dbContext);
            _roleManager = Guard.Against.Null(roleManager);
            _tokenHandler = Guard.Against.Null(tokenHandler);
        }

        public async Task<TokenDto> CreateAsync(User user, string password)
        {
            PasswordHashHelper.CreatePassword(password, out byte[] salt, out byte[] passwordHash);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;
            user = (await _dbContext.Users.AddAsync(user)).Entity;
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

            var roles = await _roleManager.GetRoleByEmail(user.Email);
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,string.Join(",",roles)),
                };
            var token = _tokenHandler.CreateAccessToken(user,claims);
            return token;
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
                _dbContext.Entry(user).State = EntityState.Detached;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
