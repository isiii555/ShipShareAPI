using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShipShareAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly JwtOptions _jwtOptions;

        public TokenHandler(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public Application.Dto.Token.TokenDto CreateAccessToken(User user,IEnumerable<Claim> claims)
        {
            var token = new Application.Dto.Token.TokenDto();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddHours(5);

            JwtSecurityToken securityToken = new(
                audience: _jwtOptions.Audience,
                issuer: _jwtOptions.Issuer,
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                claims : claims,
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
