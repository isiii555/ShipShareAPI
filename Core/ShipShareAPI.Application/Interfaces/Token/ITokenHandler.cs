using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Domain.Entities;
using System.Security.Claims;

namespace ShipShareAPI.Application.Interfaces.Token
{
    public interface ITokenHandler
    {
        Dto.Token.TokenDto CreateAccessToken(User user, IEnumerable<Claim> claims);
        string CreateRefreshToken();
        bool VerifyEmailConfirmationToken(User user, string token);
        TokenDto GenerateEmailConfirmationToken(User user);
        TokenDto GeneratePasswordResetToken(User user);
    }
}
