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
            Language = request.LanguageId,
            Success = response.Success,
            ServiceName = response.Service,
            BranchName = response.BranchName
        };

        await _context.Etickets.AddAsync(eTicket);
        await _context.SaveChangesAsync();
    }
}
