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
        Task<bool> CreateAsync(User user, string password);
    }
}
