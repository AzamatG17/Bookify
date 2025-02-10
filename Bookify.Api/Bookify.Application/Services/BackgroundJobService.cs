using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bookify.Application.Services;

internal sealed class BackgroundJobService : IBackgroundJobService
{
    private readonly IApplicationDbContext _context;

    public BackgroundJobService(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SaveBookingAsync(CreateBookingResponse response, CreateBookingRequest bookingRequest, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("User was not created correctly.");

        var booking = new Booking
        {
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
            BranchName = response.BranchName
        };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task SaveETicketAsync(EticketResponse response, CreateEticketRequest request, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new InvalidOperationException("User was not created correctly.");

        var eTicket = new ETicket
        {
            UserId = user.Id,
            User = user,
            CreatedBy = user.UserName ?? "",
            ServiceId = request.ServiceId,
            Language = request.Language,
            Success = response.Success,
            ServiceName = response.Service,
            BranchName = response.BranchName,
            CreatedTime = ParseJsonDate(response.CreatedTime),
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
            _context.Bookings.Remove(existingBooking);
            await _context.SaveChangesAsync();
        }
    }

    private static DateTime ParseJsonDate(string jsonDate)
    {
        var match = Regex.Match(jsonDate, @"\/Date\((\d+)([+-]\d{4})?\)\/");
        if (match.Success)
        {
            var milliseconds = long.Parse(match.Groups[1].Value);
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
            return dateTime;
        }

        return DateTime.UtcNow;
    }
}
