using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.Services;

public interface IBackgroundJobService
{
    Task SendETicketTelegram(EticketResponse response, Guid userId, string language);
    Task SendDeleteETicketTelegram(ETicket response, Guid userId, string language);
    Task SendBookingCodeTelegram(CreateBookingResponse response, Guid userId, string language);
    Task SendDeleteBookingTelegram(Booking response, Guid userId, string language);
    Task SaveBookingAsync(CreateBookingResponse response, CreateBookingRequest bookingRequest, Guid userId);
    Task SaveETicketAsync(EticketResponse response, CreateEticketRequest bookingRequest, Guid userId);
    Task DeleteBookingAsync(int bookingId);
    Task DeleteEticketAsync(int eTicketId);
}
