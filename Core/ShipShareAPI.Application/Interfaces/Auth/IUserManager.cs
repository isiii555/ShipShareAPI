using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Auth
{
    public interface IUserManager
    {
        Task<User?> FindByEmailAsync(string email);
        Task<TokenDto> CreateAsync(User user, string password);
        Task UpdateRefreshToken(User user,string refreshToken,DateTime accessTokenDate);
        Task<User?> GetUserWithId(Guid userId);
        Task<User?> GetUserWithRefreshToken(string refreshToken);
        Task<User?> UpdateConnectionId(string connectionId);
        Task<bool> ConfirmEmail(Guid userId, string token);
        Task SendConfirmationEmail(User user);
        Task<bool> SendForgotPasswordEmail(string email);
        Task<User?> GetMyDetails();
    }
}
