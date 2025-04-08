using Bookify.Api.Extensions;
using Bookify.Application.Extensions;
using Bookify.Infrastructure.Persistence;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/logs_.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
        .WriteTo.File("logs/errors_.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services
    .RegisterApplication(builder.Configuration)
    .RegisterApi(builder.Configuration);

builder.Services.AddHttpClient();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

#region Configure Migrations
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Ошибка при выполнении миграции базы данных");
    throw;
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDatabaseSeeder();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [ new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            RequireSsl = false,
            SslRedirect = false,
            LoginCaseSensitive = true,
            Users =
            [
                new BasicAuthAuthorizationUser
                {
                    Login = "admin",
                    PasswordClear =  "admin"
                }
            ]

        })]
});

app.UseCors("AllowAll");

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
