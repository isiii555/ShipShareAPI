using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Auth
{
    public interface IRoleManager
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role?> GetRoleByName(string roleName);
        Task<IEnumerable<Role>> GetRoleByEmail(string email);
    }
}
