namespace ShipShareAPI.API.Extensions
{
    public static class AddCorsPolicyExtension
    {
        public static IServiceCollection AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin();
                });
            });
            return services;
        }
    }
}
