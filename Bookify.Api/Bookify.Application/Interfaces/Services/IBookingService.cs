using Bookify.Application.Requests.Services;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Services;

public interface IBookingService
{
    Task<CreateBookingResponse> CreateAsync(CreateBookingRequest bookingRequest);
}
