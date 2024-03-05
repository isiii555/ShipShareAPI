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
                    policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "http://localhost:3003") // Allow requests from this origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            return services;
        }
    }
}
