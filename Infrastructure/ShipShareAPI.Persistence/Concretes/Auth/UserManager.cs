using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using ShipShareAPI.Application.Dto.Auth;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Application.Response;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class UserManager : IUserManager
    {
        private readonly ShipShareDbContext _dbContext;
        private readonly IRoleManager _roleManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IRequestUserProvider _requestUserProvider;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public UserManager(ShipShareDbContext dbContext, IRoleManager roleManager, ITokenHandler tokenHandler, IRequestUserProvider requestUserProvider, IMailService mailService, IConfiguration configuration)
        {
            _dbContext = Guard.Against.Null(dbContext);
            _roleManager = Guard.Against.Null(roleManager);
            _tokenHandler = Guard.Against.Null(tokenHandler);
            _requestUserProvider = Guard.Against.Null(requestUserProvider);
            _mailService = Guard.Against.Null(mailService);
            _configuration = Guard.Against.Null(configuration);
        }

        public async Task<bool> ConfirmEmail(Guid userId, string token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is not null)
            {
                if (_tokenHandler.VerifyEmailConfirmationToken(user, token))
                {
                    user.IsEmailConfirmed = true;
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
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

            await SendConfirmationEmail(user);

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

        public async Task<User?> FindByEmailWithoutRolesAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetMyDetails()
        {
            var userInfo = _requestUserProvider.GetUserInfo();
            var user = await _dbContext.Users.FirstOrDefaultAsync(us => us.Id == userInfo!.Id);
            return user;
        }

        public async Task<User?> GetUserWithId(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithRefreshToken(string refreshToken)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == resetPasswordRequest.Token);
            if (user is not null)
            {
                if (_tokenHandler.VerifyPasswordResetToken(user, resetPasswordRequest.Token))
                {
                    PasswordHashHelper.CreatePassword(resetPasswordRequest.Password, out byte[] salt, out byte[] passwordHash);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = salt;
                    user.PasswordResetToken = null;
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return new()
                    {
                        Message = "Your password succesfully changed",
                        IsChanged = true,
                    };
                }
                return new()
                {
                    Message = "Reset password link is expired. Please send reset password request again!",
                    IsChanged = false,
                }; 
            }
            return new()
            {
                Message = "Reset password link is expired. Please send reset password request again!",
                IsChanged = false,
            };
        }

        public async Task SendConfirmationEmail(User user)
        {
            var emailConfirmationToken = _tokenHandler.GenerateEmailConfirmationToken(user!);

            string htmlContent = File.ReadAllText("EmailTemplate.html");

            var callbackUrl = $"{_configuration["AppBaseUrl"]}/confirmemail/{user.Id}/{HttpUtility.UrlEncode(emailConfirmationToken.AccessToken)}";

            htmlContent = htmlContent.Replace("#link#", callbackUrl);

            await _mailService.SendMessageAsync(user.Email, "Email Confirmation", htmlContent, true);
        }

        public async Task<bool> SendForgotPasswordEmail(string email)
        {
            var user = await FindByEmailWithoutRolesAsync(email);
            if (user is not null)
            {
                var token = _tokenHandler.GeneratePasswordResetToken(user);
                user.PasswordResetToken = token.AccessToken;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                string htmlContent = File.ReadAllText("ForgotPassword.html");
                var callbackUrl = $"{_configuration["AppBaseUrl"]}/resetpassword/{HttpUtility.UrlEncode(token.AccessToken)}";
                htmlContent = htmlContent.Replace("#link#", callbackUrl);
                await _mailService.SendMessageAsync(user.Email, "Reset your password", htmlContent, true);
                return true;
            }
            return false;
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
