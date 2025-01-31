using Bookify.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bookify.Application.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User
                   ?? throw new InvalidOperationException("Current HTTP context does not contain a user.");

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? throw new InvalidOperationException("User does not have a NameIdentifier claim.");

        if (Guid.TryParse(userId, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"Invalid user ID format: {userId}");
    }

    public string GetUserName()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return string.Empty;
        }

        var user = httpContext.User;
        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return string.Empty;
        }

        var userName = user.FindFirst(ClaimTypes.Name)?.Value;
        return userName ?? string.Empty;
    }
}
