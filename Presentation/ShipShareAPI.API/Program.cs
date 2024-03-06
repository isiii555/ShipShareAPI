using Serilog;
using ShipShareAPI.API.Extensions;
using ShipShareAPI.API.Hubs;
using ShipShareAPI.API.Middlewares;
using ShipShareAPI.Application;
using ShipShareAPI.Infrastructure;
using ShipShareAPI.Infrastructure.Options;
using ShipShareAPI.Persistence;
using ShipShareAPI.Persistence.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
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
builder.Services.AddJwtAuthSwagger();
builder.Services.AddSignalR();

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
app.UseAddUsernameToContextMiddleware();
app.MapHub<ChatHub>("chat");
app.MapControllers();

app.Run();
