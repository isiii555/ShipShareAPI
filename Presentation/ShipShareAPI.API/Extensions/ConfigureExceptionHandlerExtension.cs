using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace ShipShareAPI.API.Extensions
{
    public static class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application,ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async (context) =>
                {
                    context.Response.StatusCode = (int)(HttpStatusCode.InternalServerError);
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        logger.LogError(contextFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = contextFeature.Error.Message,
                            Title = "Unexpected error!"
                        }));
                    };
                });
            });
        }
    }
}
