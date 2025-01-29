using Bookify.Application.Interfaces;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;

namespace Bookify.Application.Stores;

public class BookingStore
{
    private readonly IApiClient _client;

    public BookingStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CreateBookingResponse> CreateBookingForBookingServiceAsync(CreateBookingRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Bookings/CreateBooking";

        var response = await _client.PostAsync<CreateBookingResponse, CreateBookingRequest>(endpoint, request);

        return response;
    }

    public async Task<CreateBookingResponse> CreateBookingOnlinetAsync(CreateBookingRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/easyq/BookingMediator/Service1.svc/json/CreateBooking";

        var response = await _client.PostAsync<CreateBookingResponse, CreateBookingRequest>(endpoint, request);

        return response;
    }
}
