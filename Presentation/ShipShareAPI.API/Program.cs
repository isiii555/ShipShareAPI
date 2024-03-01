using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using ShipShareAPI.API;
using ShipShareAPI.API.Extensions;
using ShipShareAPI.Application;
using ShipShareAPI.Infrastructure;
using ShipShareAPI.Infrastructure.Options;
using ShipShareAPI.Persistence;
using ShipShareAPI.Persistence.Options;
using Swashbuckle.AspNetCore.Filters;
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
builder.Services.AddApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.UseCors();

//app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated is not null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username?.ToString() ?? null);
    await next.Invoke();
});


app.MapControllers();

app.Run();
