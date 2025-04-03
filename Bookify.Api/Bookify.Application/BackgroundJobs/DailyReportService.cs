using Bookify.Application.Interfaces;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookify.Application.BackgroundJobs;

internal sealed class DailyReportService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DailyReportService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider)); 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRunTime = now.Date.AddHours(8);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            var delay = nextRunTime - now;
            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await SendDailyReportAsync();
            }
        }
    }

    private async Task SendDailyReportAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        var bookings = await context.Bookings
            .Where(x => x.StartDate.Date == DateTime.UtcNow.Date)
            .Include(u => u.User)
            .ToListAsync();

        foreach( var booking in bookings )
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
