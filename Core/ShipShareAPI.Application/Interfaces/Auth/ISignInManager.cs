using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Auth
{
    public interface ISignInManager
    {
        Task<Dto.Token.TokenDto> SignInAsync(User user,string password);
        Task<Dto.Token.TokenDto> RefreshTokenSignInAsync();

    }
}
