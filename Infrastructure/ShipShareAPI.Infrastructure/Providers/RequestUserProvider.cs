using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using ShipShareAPI.Application.Interfaces.Providers;
using ShipShareAPI.Domain.ValueObjects;
using System.Security.Claims;

namespace ShipShareAPI.Infrastructure.Providers
{
    public class RequestUserProvider : IRequestUserProvider
    {
        private readonly HttpContext? _httpContext;

        public RequestUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public UserInfo? GetUserInfo()
        {
            var name = _httpContext!.User.FindFirstValue(ClaimTypes.Name)!;
            var id = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)!;
            var role = _httpContext.User.FindFirstValue(ClaimTypes.Role)!;
            var userInfo = new UserInfo(name,id,role);
            return userInfo;
        }
    }
}
