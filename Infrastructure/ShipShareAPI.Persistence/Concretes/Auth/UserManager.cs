﻿using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System.Security.Claims;

namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class UserManager : IUserManager
    {
        private readonly ShipShareDbContext _dbContext;
        private readonly IRoleManager _roleManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IRequestUserProvider _requestUserProvider;

        public UserManager(ShipShareDbContext dbContext, IRoleManager roleManager, ITokenHandler tokenHandler, IRequestUserProvider requestUserProvider)
        {
            _dbContext = Guard.Against.Null(dbContext);
            _roleManager = Guard.Against.Null(roleManager);
            _tokenHandler = Guard.Against.Null(tokenHandler);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
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

            var roles = (await _roleManager.GetRoleByEmail(user.Email)).ToList();
            var roleNames = new List<string>();
            roles.ForEach(r =>
            {
                roleNames.Add(r!.Name);
            });
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,string.Join(",",roleNames)),
                };
            var token = _tokenHandler.CreateAccessToken(user, claims);
            return token;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithId(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithRefreshToken(string refreshToken)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> UpdateConnectionId(string connectionId)
        {
            var userInfo = _requestUserProvider.GetUserInfo();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo!.Id);
            if (user is not null)
            {
                user.ConnectionId = connectionId;
                user = _dbContext.Users.Update(user).Entity;
                await _dbContext.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task UpdateRefreshToken(User user, string refreshToken, DateTime accessTokenDate)
        {
            if (user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireDate = accessTokenDate.AddMinutes(10);
                var trackedUser = await _dbContext.Users.FindAsync(user.Id);

                if (trackedUser != null)
                {
                    trackedUser.RefreshToken = refreshToken;
                    trackedUser.RefreshTokenExpireDate = accessTokenDate.AddMinutes(10);

                    // Set EntityState to Modified
                    _dbContext.Entry(trackedUser).State = EntityState.Modified;

                    await _dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("User not found");
            }
        }
    }
}
