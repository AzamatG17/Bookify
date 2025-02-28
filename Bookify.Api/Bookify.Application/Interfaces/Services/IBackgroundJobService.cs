using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Services;

public interface IBackgroundJobService
{
    Task SendETicketTelegram(EticketResponse response, Guid userId, string language, DateTime dateTime);
    Task SendDeleteETicketTelegram(EticketResponse response, Guid userId, string language);
    Task SendBookingCodeTelegram(CreateBookingResponse response, Guid userId, string language);
    Task SendDeleteBookingTelegram(BookingDto response, Guid userId, string language);
    Task SaveBookingAsync(CreateBookingResponse response, CreateBookingRequest bookingRequest, Guid userId);
    Task SaveETicketAsync(EticketResponse response, CreateEticketRequest bookingRequest, Guid userId, DateTime dateTime);
    Task DeleteBookingAsync(int bookingId);
    Task DeleteEticketAsync(int eTicketId);
}
