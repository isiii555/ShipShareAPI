using ShipShareAPI.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Auth
{
    public class SignInManager : ISignInManager
    {
        public Task<bool> SignInAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
