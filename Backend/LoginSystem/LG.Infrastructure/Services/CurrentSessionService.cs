using LG.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LG.Infrastructure.Services
{
    public class CurrentSessionService(IHttpContextAccessor httpContextAccessor) : ICurrentSessionService
    {
        public int Get()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
