using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
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

        public Application.Dto.Token.Token CreateAccessToken()
        {
            var token = new Application.Dto.Token.Token();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddMinutes(15);

            JwtSecurityToken securityToken = new(
                audience: _jwtOptions.Audience,
                issuer: _jwtOptions.Issuer,
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
