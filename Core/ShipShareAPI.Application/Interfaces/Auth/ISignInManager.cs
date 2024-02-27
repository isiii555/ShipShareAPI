using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Auth
{
    public interface ISignInManager
    {
        Task<bool> SignInAsync(string email,string password);
    }
}
