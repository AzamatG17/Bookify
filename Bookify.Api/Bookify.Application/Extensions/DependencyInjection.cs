using Bookify.Application.Interfaces;
using Bookify.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISmsCodeService, SmsCodeService>();
        services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
        return services;
    }
}
