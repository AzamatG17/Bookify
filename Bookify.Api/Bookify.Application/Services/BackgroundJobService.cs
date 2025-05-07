using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class BackgroundJobService : IBackgroundJobService
{
    private readonly IApplicationDbContext _context;
    private readonly ITelegramService _telegramService;

    public BackgroundJobService(IApplicationDbContext context, ITelegramService telegramService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _telegramService = telegramService ?? throw new ArgumentNullException(nameof(telegramService));
    }

    public async Task SendETicketTelegram(EticketResponse response, Guid userId, string language, DateTime dateTime)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var message = language switch
        {
            "uz" => $"Sizning elektron chiptangiz yaratildi! ✅\n" +
                    $"Chipta kodi: {response.Number}\n" +
                    $"Xizmat: {response.Service}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Qabul qilingan sana: {dateTime.ToString("dd.MM.yyyy HH:mm")}",

            "ru" => $"Ваш электронный билет успешно создан! ✅\n" +
                    $"Код билета: {response.Number}\n" +
                    $"Услуга: {response.Service}\n" +
                    $"Филиал: {response.BranchName}\n" +
                    $"Дата получения: {dateTime.ToString("dd.MM.yyyy HH:mm")}",

            "en" => $"Your electronic ticket has been created! ✅\n" +
                    $"Ticket code: {response.Number}\n" +
                    $"Service: {response.Service}\n" +
                    $"Branch: {response.BranchName}\n" +
                    $"Received on: {dateTime.ToString("dd.MM.yyyy HH:mm")}",

            _ => $"Sizning elektron chiptangiz yaratildi! ✅\n" +
                    $"Chipta kodi: {response.Number}\n" +
                    $"Xizmat: {response.Service}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Qabul qilingan sana: {dateTime.ToString("dd.MM.yyyy HH:mm")}"
        };

        await _telegramService.SendMessageAsync(user.ChatId, message);
    }

    public async Task SendDeleteETicketTelegram(EticketResponse response, Guid userId, string language)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var message = language switch
        {
            "uz" => $"Sizning elektron chiptangiz o‘chirildi! ❌\n" +
                    $"Chipta kodi: {response.Number}\n" +
                    $"Xizmat: {response.Service}\n" +
                    $"Filial: {response.BranchName}",

            "ru" => $"Ваш электронный билет был удалён! ❌\n" +
                    $"Код билета: {response.Number}\n" +
                    $"Услуга: {response.Service}\n" +
                    $"Филиал: {response.BranchName}",

            "en" => $"Your electronic ticket has been deleted! ❌\n" +
                    $"Ticket code: {response.Number}\n" +
                    $"Service: {response.Service}\n" +
                    $"Branch: {response.BranchName}",

            _ => $"Sizning elektron chiptangiz o‘chirildi! ❌\n" +
                    $"Chipta kodi: {response.Number}\n" +
                    $"Xizmat: {response.Service}\n" +
                    $"Filial: {response.BranchName}"
        };

        await _telegramService.SendMessageAsync(user.ChatId, message);
    }

    public async Task SendBookingCodeTelegram(CreateBookingResponse response, Guid userId, string language)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var message = language switch
        {
            "uz" => $"Sizning chiptangiz yaratildi! ✅\n" +
                    $"Chipta kodi: {response.BookingCode}\n" +
                    $"Xizmat: {response.ServiceName}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Sana: {response.BookingDate.Date:dd.MM.yyyy}\n" +
                    $"Vaqt: {response.BookingTime}\n" +
                    $"Chipta olish uchun terminalga ushbu kodni kiriting.",

            "ru" => $"Ваш билет успешно создан! ✅\n" +
                    $"Код билета: {response.BookingCode}\n" +
                    $"Услуга: {response.ServiceName}\n" +
                    $"Филиал: {response.BranchName}\n" +
                    $"Дата: {response.BookingDate.Date:dd.MM.yyyy}\n" +
                    $"Время: {response.BookingTime}\n" +
                    $"Введите этот код в терминале, чтобы получить билет.",

            "en" => $"Your ticket has been created! ✅\n" +
                    $"Ticket code: {response.BookingCode}\n" +
                    $"Service: {response.ServiceName}\n" +
                    $"Branch: {response.BranchName}\n" +
                    $"Date: {response.BookingDate.Date:dd.MM.yyyy}\n" +
                    $"Time: {response.BookingTime}\n" +
                    $"Enter this code in the terminal to get a ticket.",

            _ => $"Sizning chiptangiz yaratildi! ✅\n" +
                    $"Chipta kodi: {response.BookingCode}\n" +
                    $"Xizmat: {response.ServiceName}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Sana: {response.BookingDate.Date:dd.MM.yyyy}\n" +
                    $"Vaqt: {response.BookingTime}\n" +
                    $"Chipta olish uchun terminalga ushbu kodni kiriting."
        };

        await _telegramService.SendMessageAsync(user.ChatId, message);
    }

    public async Task SendDeleteBookingTelegram(BookingDto response, Guid userId, string language)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var message = language switch
        {
            "uz" => $"Sizning chiptangiz o‘chirildi. ❌\n" +
                    $"Chipta kodi: {response.BookingCode}\n" +
                    $"Xizmat: {response.ServiceName}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Sana: {response.StartDate.Date:dd.MM.yyyy}\n" +
                    $"Vaqt: {response.StartTime}",

            "ru" => $"Ваш билет был удалён. ❌\n" +
                    $"Код билета: {response.BookingCode}\n" +
                    $"Услуга: {response.ServiceName}\n" +
                    $"Филиал: {response.BranchName}\n" +
                    $"Дата: {response.StartDate.Date:dd.MM.yyyy}\n" +
                    $"Время: {response.StartTime}",

            "en" => $"Your ticket has been deleted. ❌\n" +
                    $"Ticket code: {response.BookingCode}\n" +
                    $"Service: {response.ServiceName}\n" +
                    $"Branch: {response.BranchName}\n" +
                    $"Date: {response.StartDate.Date:dd.MM.yyyy}\n" +
                    $"Time: {response.StartTime}",

            _ => $"Sizning chiptangiz o‘chirildi. ❌\n" +
                    $"Chipta kodi: {response.BookingCode}\n" +
                    $"Xizmat: {response.ServiceName}\n" +
                    $"Filial: {response.BranchName}\n" +
                    $"Sana: {response.StartDate.Date:dd.MM.yyyy}\n" +
                    $"Vaqt: {response.StartTime}"
        };

        await _telegramService.SendMessageAsync(user.ChatId, message);
    }

    public async Task SaveBookingAsync(CreateBookingResponse response, CreateBookingRequest bookingRequest, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var booking = new Booking
        {
            BookingId = response.BookingId,
            UserId = user.Id,
            User = user,
            CreatedBy = user.UserName ?? "",
            ServiceId = bookingRequest.ServiceId,
            StartDate = bookingRequest.StartDate,
            StartTime = bookingRequest.StartTime,
            Language = bookingRequest.Language,
            BookingCode = response.BookingCode,
            Success = response.Success,
            ServiceName = response.ServiceName,
            BranchName = response.BranchName,
            ClientId = response.ClientId ?? null
        };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task SaveETicketAsync(EticketResponse response, CreateEticketRequest request, Guid userId, DateTime dateTime)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("Пользователь не был создан правильно.");

        var eTicket = new ETicket
        {
            ETicketId = response.TicketId,
            UserId = user.Id,
            User = user,
            CreatedBy = user.UserName ?? "",
            ServiceId = request.ServiceId,
            Language = request.Language,
            Success = response.Success,
            ServiceName = response.Service,
            BranchName = response.BranchName,
            CreatedTime = dateTime,
            Message = response.Message ?? "",
            Number = response.Number,
            ValidUntil = response.ValidUntil,
            ShowArriveButton = response.ShowArriveButton,
            TicketId = response.TicketId,
        };

        await _context.Etickets.AddAsync(eTicket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookingAsync(int bookingId)
    {
        var existingBooking = await _context.Bookings.FindAsync(bookingId);

        if (existingBooking != null)
        {
            existingBooking.IsActive = false;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEticketAsync(int eTicketId)
    {
        var eTicket = await _context.Etickets.FindAsync(eTicketId);

        if (eTicket != null)
        {
            eTicket.IsActive = false;
        }

        await _context.SaveChangesAsync();
    }
}
