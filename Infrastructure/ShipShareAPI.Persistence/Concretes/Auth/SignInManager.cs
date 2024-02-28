using Ardalis.GuardClauses;
using Microsoft.IdentityModel.Tokens;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Persistence.Context;
using System.Security.Claims;


namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class SignInManager : ISignInManager
    {
        private readonly ShipShareDbContext _dbContext;
        private readonly IUserManager _userManager;
        private readonly ITokenHandler _tokenHandler;

        public SignInManager(ShipShareDbContext dbContext, IUserManager userManager, ITokenHandler tokenHandler)
        {
            _dbContext = Guard.Against.Null(dbContext);
            _userManager = Guard.Against.Null(userManager);
            _tokenHandler = Guard.Against.Null(tokenHandler);
        }

        public async Task<TokenDto> SignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,string.Join(",",user!.Roles!)),
                };
                return _tokenHandler.CreateAccessToken(user,claims);
            }
            throw new Exception("Sign in operation failed!");
        }
    }
}
