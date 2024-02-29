using ShipShareAPI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Providers
{
    public interface IRequestUserProvider
    {
        UserInfo? GetUserInfo();
    }
}
