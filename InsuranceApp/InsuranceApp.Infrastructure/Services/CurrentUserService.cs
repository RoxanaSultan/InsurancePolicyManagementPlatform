using InsuranceApp.Application.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InsuranceApp.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUserService> logger) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<CurrentUserService> _logger = logger;

    public string? GetUserId()
    {
        _logger.LogInformation("Retrieving current user id.");

        return _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public ClaimsIdentity? GetUserIdentity()
    {
        _logger.LogInformation("Retrieving current user identity.");

        return _httpContextAccessor.HttpContext?
            .User?
            .Identity as ClaimsIdentity;
    }
}
