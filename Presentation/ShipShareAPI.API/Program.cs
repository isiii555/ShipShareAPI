using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using ShipShareAPI.API;
using ShipShareAPI.API.Extensions;
using ShipShareAPI.Infrastructure;
using ShipShareAPI.Infrastructure.Options;
using ShipShareAPI.Persistence;
using ShipShareAPI.Persistence.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddSerilog();
builder.Services.AddCorsExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Token"));
builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddJwtAuth(builder.Configuration);

var app = builder.Build();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.UseCors();

//app.UseHttpLogging();

//app.Use(async (context, next) =>
//{
//    var username = context.User?.Identity?.IsAuthenticated is not null || true ? context.User.Identity.Name : null;
//    LogContext.PushProperty("user_name", username?.ToString() ?? null);
//    await next.Invoke();
//});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
