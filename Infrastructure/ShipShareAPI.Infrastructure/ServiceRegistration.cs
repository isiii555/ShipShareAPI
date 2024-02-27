using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShipShareAPI.Application.Interfaces.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler,Services.Token.TokenHandler>();
            return services;
        }
    }
}
