using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Requests.Stores;
using Bookify.Application.Responses;

namespace Bookify.Application.Stores;

public class EticketStore : IEticketStore
{
    private readonly IApiClient _client;

    public EticketStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<EticketResponse> CreateTicketForBookingServiceAsync(EticketRequest request, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var endpoint = $"{baseUrl}/api/E_Ticket/CreateNewTicket";

        var response = await _client.PostAsync<EticketResponse, EticketRequest>(endpoint, request);

        return response;
    }

    public async Task<EticketResponse> CreateTicketForOnlinetAsync(ETicketOnlinetRequest request, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/CreateNewTicket";

        var response = await _client.PostAsync<EticketResponse, ETicketOnlinetRequest>(endpoint, request);

        return response;
    }
}
