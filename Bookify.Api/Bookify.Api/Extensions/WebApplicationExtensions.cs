using Bookify.Api.Helpers;
using Bookify.Api.Middlewares;
using Bookify.Domain_.Interfaces;

namespace Bookify.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDatabaseSeeder(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        //DatabaseSeeder.SeedDatabase(context);

        return app;
    }

    public static WebApplication UseErrorHandler(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        return app;
    }
}
