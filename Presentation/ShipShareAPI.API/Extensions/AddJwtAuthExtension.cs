using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ShipShareAPI.API.Extensions
{
    public static class AddJwtAuthExtension
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new()
                        {
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,


                            ValidAudience = configuration["Token:Audience"],
                            ValidIssuer = configuration["Token:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]!)),
                            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires is not null ? expires > DateTime.UtcNow : false,
                        };
                    });
            return services;
        }
    }
}
