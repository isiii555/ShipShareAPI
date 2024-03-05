using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Repositories;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Persistence.Concretes.Auth;
using ShipShareAPI.Persistence.Concretes.Repositories;
using ShipShareAPI.Persistence.Concretes.Services;
using ShipShareAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShipShareDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("ShipShareConStr")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).EnableSensitiveDataLogging();
            });
            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<ISignInManager, SignInManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IConversationRepository,ConversationRepository>();
            services.AddScoped<ISenderPostRepository, SenderPostsRepository>();
            services.AddScoped<ITravellerPostRepository, TravellerPostRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUploadImageToStorageService,UploadImageToStorageService>();
            return services;
        }
    }
}
