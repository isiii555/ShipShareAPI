using ShipShareAPI.API.Middlewares;

namespace ShipShareAPI.API.Extensions
{
    public static class AddUsernameToContextMiddlewareExtension
    {
        public static IApplicationBuilder UseAddUsernameToContextMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddUsernameToContextMiddleware>(); ;
        }
    }
}
