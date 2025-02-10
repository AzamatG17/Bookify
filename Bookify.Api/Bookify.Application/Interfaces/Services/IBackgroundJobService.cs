using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.Services;

public interface IBackgroundJobService
{
    Task SaveBookingAsync(CreateBookingResponse response, CreateBookingRequest bookingRequest, Guid userId);
    Task SaveETicketAsync(EticketResponse response, CreateEticketRequest bookingRequest, Guid userId);
    Task DeleteBookingAsync(int bookingId);
}
