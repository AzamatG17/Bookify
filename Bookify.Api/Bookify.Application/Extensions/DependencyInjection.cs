using Bookify.Application.BackgroundJobs;
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
using Hangfire;
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
        services.AddScoped<IListFreeTimesStore, ListFreeTimesStore>();
        services.AddScoped<IListFreeDaysStore, ListFreeDaysStore>();
        services.AddScoped<IBookingStore, BookingStore>();
        services.AddScoped<IEticketStore, EticketStore>();

        services.AddScoped<IPredefinedTextService, PredefinedTextService>();
        services.AddScoped<IServiceRatingService, ServiceRatingService>();
        services.AddScoped<IEticketService, EticketService>();
        services.AddScoped<IFreeTimeService, FreeTimeService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICompaniesService, CompaniesService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISmsCodeService, SmsCodeService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();

        services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHostedService<DailyReportService>();

        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(options =>
        {
            options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddHangfireServer();
    }
}
