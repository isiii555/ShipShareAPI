using Microsoft.Extensions.DependencyInjection;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Infrastructure.Providers;
using ShipShareAPI.Infrastructure.Services.Mail;


namespace ShipShareAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler,Services.Token.TokenHandler>();
            services.AddScoped<IRequestUserProvider, RequestUserProvider>();
            services.AddScoped<IMailService,MailService>();
            return services;
        }
    }
}
