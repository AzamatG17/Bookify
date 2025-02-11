using Bookify.Application.Models;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;

namespace Bookify.Application.Interfaces.Stores;

public interface IBookingStore
{
    Task<CreateBookingResponse> CreateBookingForBookingServiceAsync(BookingForBookingServiceRequest request, string baseUrl);
    Task<CreateBookingResponse> CreateBookingOnlinetAsync(BookingRequest request, string baseUrl);
    Task<ResultBooking> DeleteBookingForBookingServiceAsync(
        string baseUrl, string bookingCode, string clientId, string languageShortId, string startDate);
    Task<ResultBooking> DeleteBookingForOnlinetAsync(DeleteBookingRequest request, string baseUrl);
}
