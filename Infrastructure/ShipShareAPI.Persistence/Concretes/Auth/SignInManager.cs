using Ardalis.GuardClauses;
using Microsoft.IdentityModel.Tokens;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Persistence.Context;
using ShipShareAPI.Persistence.Helpers;
using System.Data;
using System.Security.Claims;


namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class SignInManager : ISignInManager
    {
        private readonly IUserManager _userManager;
        private readonly ITokenHandler _tokenHandler;

        public SignInManager(IUserManager userManager, ITokenHandler tokenHandler, ShipShareDbContext dbContext)
        {
            _userManager = Guard.Against.Null(userManager);
            _tokenHandler = Guard.Against.Null(tokenHandler);
        }

        public async Task<TokenDto> RefreshTokenSignInAsync(string refreshToken)
        {
            var user = await _userManager.GetUserWithRefreshToken(refreshToken);
            if (user is not null && user.RefreshTokenExpireDate > DateTime.UtcNow)
            {
                List<string> roles = new();
                user!.Roles!.ForEach(r =>
                {
                    roles.Add(r.Name);
                });
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,string.Join(",",roles)),
                };
                var token = _tokenHandler.CreateAccessToken(user, claims);
                await _userManager.UpdateRefreshToken(user, token.RefreshToken, token.Expiration);
                return token;
            }
            throw new Exception("User not found");
        }

        public async Task<TokenDto> SignInAsync(User user,string password)
        {
            List<string> roles = new();
            user!.Roles!.ForEach(r =>
            {
                roles.Add(r.Name);
            });
            if (user is not null)
            {
                var check = PasswordHashHelper.ComparePassword(password, user.PasswordHash, user.PasswordSalt);
                if (check)
                {
                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name,user.Username),
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Role,string.Join(",",roles)),
                        };
                    var token = _tokenHandler.CreateAccessToken(user, claims);
                    await _userManager.UpdateRefreshToken(user, token.RefreshToken, token.Expiration);
                    return token;
                }
                else
                    throw new Exception("Password is wrong!");
            }
            throw new Exception("Sign in operation failed!");
        }

    }
}
