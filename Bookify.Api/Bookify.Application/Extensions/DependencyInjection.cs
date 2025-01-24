using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Mappings;
using Bookify.Application.Services;
using Bookify.Application.Stores;
using Bookify.Application.Validations.Auth;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Bookify.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(CompaniesMappings).Assembly);
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        services.AddFluentValidationAutoValidation();

        services.AddScoped<IBranchStore, BranchStore>();
        services.AddScoped<IServiceStore, ServiceStore>();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICompaniesService, CompaniesService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISmsCodeService, SmsCodeService>();
        services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
        return services;
    }
}
