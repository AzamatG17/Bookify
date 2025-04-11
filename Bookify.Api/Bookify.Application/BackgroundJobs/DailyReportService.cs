using Bookify.Application.Interfaces;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.BackgroundJobs;

internal sealed class DailyReportService(IServiceProvider serviceProvider, ILogger<DailyReportService> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly ILogger<DailyReportService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("⏱ DailyReportService started");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                var morningRun = now.Date.AddHours(3);
                var eveningRun = now.Date.AddHours(11);

                DateTime nextRunTime;
                Func<CancellationToken, Task> actionToRun;

                if (now < morningRun)
                {
                    nextRunTime = morningRun;
                    actionToRun = SendDailyReportAsync;
                }
                else if (now < eveningRun)
                {
                    nextRunTime = eveningRun;
                    actionToRun = SendDayBaforeReportAsync;
                }
                else
                {
                    nextRunTime = morningRun.AddDays(1);
                    actionToRun = SendDailyReportAsync;
                }

                var delay = nextRunTime - now;

                if (delay.TotalMilliseconds <= 0)
                {
                    delay = TimeSpan.FromMinutes(1);
                    _logger.LogWarning("⚠️ Negative or zero delay. Adjusted to 1 minute.");
                }

                _logger.LogInformation("⌛ Waiting {Delay} until next run at {NextRunTime}", delay, nextRunTime);

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("📤 Running scheduled task at {Time}", nextRunTime);
                        await actionToRun(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Error while sending report.");
                    }
                }
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("🛑 DailyReportService cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❗ Unexpected error");
        }

        _logger.LogInformation("✅ DailyReportService finished");
    }

    private async Task SendDailyReportAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        var today = DateTime.UtcNow.Date;
        var bookings = await context.Bookings
            .Where(x => x.StartDate.Date == today)
            .Include(u => u.User)
            .ToListAsync(stoppingToken);

        foreach (var booking in bookings)
        {
            var message = GenerateMessage(booking);
            await telegramService.SendMessageAsync(booking.User.ChatId, message);
        }
    }

    private async Task SendDayBaforeReportAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        var day = DateTime.UtcNow.Date.AddDays(1);
        var bookings = await context.Bookings
            .Where(x => x.StartDate.Date == day)
            .Include(u => u.User)
            .ToListAsync(stoppingToken);

        foreach (var booking in bookings)
        {
            var message = GenerateMessage(booking);
            await telegramService.SendMessageAsync(booking.User.ChatId, message);
        }
    }

    public static string GenerateMessage(Booking booking)
    {
        var message = booking.User.Language switch
        {
            "ru" => $"Здравствуйте, {booking.User.LastName} {booking.User.FirstName}!\n" +
                    $"Напоминаем вам о вашем бронировании на {booking.StartDate:dd.MM.yyyy} в {booking.StartTime}.\n" +
                    $"Код билета: {booking.BookingCode}\n" +
                    $"Услуга: {booking.ServiceName}.\n" +
                    $"Филиал: {booking.BranchName}.",

            "uz" => $"Assalomu alaykum, {booking.User.LastName} {booking.User.FirstName}!\n" +
                    $"Sizning {booking.StartDate:dd.MM.yyyy} kuni soat {booking.StartTime} dagi broningiz eslatib qo'yamiz.\n" +
                    $"Chipta kodi: {booking.BookingCode}\n" +
                    $"Xizmat: {booking.ServiceName}.\n" +
                    $"Filial: {booking.BranchName}.",

            "en" => $"Hello, {booking.User.LastName} {booking.User.FirstName}!\n" +
                    $"This is a reminder about your booking on {booking.StartDate:dd.MM.yyyy} at {booking.StartTime}.\n" +
                    $"Ticket code: {booking.BookingCode}\n" +
                    $"Service: {booking.ServiceName}.\n" +
                    $"Branch: {booking.BranchName}.",

            _ =>    $"Здравствуйте, {booking.User.LastName} {booking.User.FirstName}!\n" +
                    $"Напоминаем вам о вашем бронировании на {booking.StartDate:dd.MM.yyyy} в {booking.StartTime}.\n" +
                    $"Код билета: {booking.BookingCode}\n" +
                    $"Услуга: {booking.ServiceName}.\n" +
                    $"Филиал: {booking.BranchName}.",
        };

        return message;
    }
}
