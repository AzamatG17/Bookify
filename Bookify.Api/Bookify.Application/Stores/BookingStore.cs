using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Bookify.Application.Stores;

public class BookingStore : IBookingStore
{
    private readonly IApiClient _client;

    public BookingStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CreateBookingResponse> CreateBookingForBookingServiceAsync(BookingForBookingServiceRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Bookings/CreateBooking";

        var response = await _client.PostAsync<CreateBookingResponse, BookingForBookingServiceRequest>(endpoint, request);

        return response;
    }

    public async Task<CreateBookingResponse> CreateBookingOnlinetAsync(BookingRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/CreateBooking";

        var response = await _client.PostAsync<BookingOnlinetResponse, BookingRequest>(endpoint, request);

        return MapToBookingResponse(response);
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
        };
    }
}
