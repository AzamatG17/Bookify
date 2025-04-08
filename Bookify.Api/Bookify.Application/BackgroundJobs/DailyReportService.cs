using Bookify.Application.Interfaces;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.BackgroundJobs;

internal sealed class DailyReportService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyReportService> _logger;

    public DailyReportService(IServiceProvider serviceProvider, ILogger<DailyReportService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("⏱ DailyReportService started");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRunTime = now.Date.AddHours(8);

                if (now > nextRunTime)
                    nextRunTime = nextRunTime.AddDays(1);

                var delay = nextRunTime - now;

                if (delay.TotalMilliseconds <= 0)
                {
                    delay = TimeSpan.FromMinutes(1);
                    _logger.LogWarning("⚠️ Calculated negative or zero delay. Adjusted to 1 minute.");
                }

                _logger.LogInformation("⌛ Waiting {Delay} until next run at {NextRunTime}", delay, nextRunTime);

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("📤 Sending daily reports...");
                        await SendDailyReportAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Error while sending daily report.");
                    }
                }
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("🛑 DailyReportService cancelled (app shutdown)");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❗ Unexpected error in DailyReportService");
        }

        _logger.LogInformation("✅ DailyReportService finished");
    }

    private async Task SendDailyReportAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        var bookings = await context.Bookings
            .Where(x => x.StartDate.Date == DateTime.UtcNow.Date)
            .Include(u => u.User)
            .ToListAsync(stoppingToken);

        foreach (var booking in bookings)
        {
            var message = GenerateMessageAsync(booking);
            await telegramService.SendMessageAsync(booking.User.ChatId, message);
        }
    }

    public static string GenerateMessageAsync(Booking booking)
    {
        var message = booking.User.Language switch
        {
            "ru" => $"Здравствуйте, {booking.User.LastName} {booking.User.FirstName}!" +
                    $"Напоминаем вам о вашем бронировании на {booking.StartDate:dd.MM.yyyy} в {booking.StartTime}." +
                    $"Услуга: {booking.ServiceName}.\n" +
                    $"Филиал: {booking.BranchName}.\n\n",

            "uz" => $"Assalomu alaykum, {booking.User.LastName} {booking.User.FirstName}!" +
                    $"Sizning {booking.StartDate:dd.MM.yyyy} kuni soat {booking.StartTime} dagi broningiz eslatib qo'yamiz." +
                    $"Xizmat: {booking.ServiceName}.\n" +
                    $"Filial: {booking.BranchName}.\n\n",

            "en" => $"Hello, {booking.User.LastName} {booking.User.FirstName}!" +
                    $"This is a reminder about your booking on {booking.StartDate:dd.MM.yyyy} at {booking.StartTime}." +
                    $"Service: {booking.ServiceName}.\n" +
                    $"Branch: {booking.BranchName}.\n\n",

            _ => $"Здравствуйте, {booking.User.LastName} {booking.User.FirstName}! Напоминаем вам о вашем бронировании на {booking.StartDate:dd.MM.yyyy} в {booking.StartTime}." +
                    $"Услуга: {booking.ServiceName}.\n" +
                    $"Фили��л: {booking.BranchName}.\n\n"
        };

        return message;
    }
}
