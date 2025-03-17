using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Services;

public interface IBookingService
{
    Task<object> GetBookingStatusAsync(GetBookingStatusRequest request);
    Task<CreateBookingResponse> CreateAsync(CreateBookingRequest bookingRequest);
    Task DeleteAsync(GetBookingRequest request);
}
