using Serilog.Context;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;

namespace ShipShareAPI.API.Middlewares
{
    public class AddUsernameToContextMiddleware
    {
        private readonly RequestDelegate _next;

        public AddUsernameToContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var username = context.User?.Identity?.IsAuthenticated is not null || true ? context.User.FindFirstValue(ClaimTypes.Email) : null;
            LogContext.PushProperty("user_name", username?.ToString() ?? null);
            await _next.Invoke(context);
        }
    }
}
