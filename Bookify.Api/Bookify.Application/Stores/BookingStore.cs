using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Models;
using Bookify.Application.Requests.Services;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;
using System.Globalization;

namespace Bookify.Application.Stores;

public class BookingStore : IBookingStore
{
    private readonly IApiClient _client;

    public BookingStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<BookingStatusResponse> GetBookingIsActive(string BookingCode, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Bookings/checkBooking?bookingNumber={BookingCode}";

        return await _client.GetAsync<BookingStatusResponse>(endpoint);
    }

    public async Task<object> GetBookingStatusAsync(GetBookingStatusRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Bookings/infoBooking?bookingNumber={request.BookingCode}";

        return await _client.GetStringAsync<object>(endpoint);
    }

    public async Task<CreateBookingResponse> CreateBookingForBookingServiceAsync(BookingForBookingServiceRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Bookings/CreateBooking";

        return await _client.PostAsync<CreateBookingResponse, BookingForBookingServiceRequest>(endpoint, request);
    }

    public async Task<CreateBookingResponse> CreateBookingOnlinetAsync(BookingRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/CreateBooking";

        var response = await _client.PostAsync<BookingOnlinetResponse, BookingRequest>(endpoint, request);

        return MapToBookingResponse(response);
    }

    public async Task<ResultBooking> DeleteBookingForBookingServiceAsync(
        string baseUrl, string bookingCode, string clientId, string languageShortId, string startDate)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        if (string.IsNullOrEmpty(bookingCode) || string.IsNullOrEmpty(clientId) ||
            string.IsNullOrEmpty(languageShortId) || string.IsNullOrEmpty(startDate))
            throw new ArgumentException("One or more required parameters are missing.");

        var endpoint = $"{baseUrl}/api/Bookings/DeleteBooking?" +
                       $"bookingCode={bookingCode}&clientId={clientId}&languageShortId={languageShortId}&startDate={startDate}";

        return await _client.PostAsync<ResultBooking>(endpoint);
    }

    public async Task<ResultBooking> DeleteBookingForOnlinetAsync(DeleteBookingRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/DeleteBooking";

        var result = await _client.PostAsync<DeleteBookingResult, DeleteBookingRequest>(endpoint, request);

        return new ResultBooking
        {
            Code = result.Code,
            Message = result.Message,
            Success = result.Success,
        };
    }

    private static CreateBookingResponse MapToBookingResponse(BookingOnlinetResponse bookingOnlinetResponse)
    {
        DateTime bookingDate = DateTime.MinValue;

        if (DateTime.TryParseExact(bookingOnlinetResponse.BookingDate, "yyyyMMdd",
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            bookingDate = parsedDate;
        }

        return new CreateBookingResponse
        {
            BookingId = bookingOnlinetResponse.BookingId,
            BookingCode = bookingOnlinetResponse.BookingCode,
            BookingDate = bookingDate,
            BookingTime = TimeSpan.TryParse(bookingOnlinetResponse.BookingTime, out TimeSpan parseTime) ? parseTime : TimeSpan.Zero,
            BranchName = bookingOnlinetResponse.BranchName,
            ServiceName = bookingOnlinetResponse.ServiceName,
            Success = bookingOnlinetResponse.Success,
            ClientId = bookingOnlinetResponse.ClientId,
        };
    }
}
