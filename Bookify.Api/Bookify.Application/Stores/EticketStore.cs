﻿using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Models;
using Bookify.Application.Requests.Services;
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

    public async Task<object> GetEtickertStatusBookingServiceAsync(EticketStatusRequest request, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/E_Ticket/info?BranchId={request.BranchId}&TicketId={request.TicketId}";

        return await _client.GetStringAsync<object>(endpoint);
    }

    public async Task<EticketResponse> CreateTicketForBookingServiceAsync(EticketRequest request, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var endpoint = $"{baseUrl}/api/E_Ticket/CreateNewTicket";

        return await _client.PostEncodeAsync<EticketResponse, EticketRequest>(endpoint, request);
    }

    public async Task<EticketResponse> CreateTicketForOnlinetAsync(ETicketOnlinetRequest request, string baseUrl)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/CreateNewTicket";

        return await _client.PostEncodeAsync<EticketResponse, ETicketOnlinetRequest>(endpoint, request);
    }

    public async Task<DeleteResponse> DeleteBookingServiceAsync(string baseUrl, int branchId, string number)
    {
        var endpoint = $"{baseUrl}/api/E_Ticket/DeleteTicket?branchId={branchId}&number={number}";

        return await _client.PostAsync<DeleteResponse, object>(endpoint, null);
    }

    public async Task<DeleteResponse> DeleteOnlinetAsync(string baseUrl, string UserId, string number)
    {
        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/DeleteTicket?number={number}&deviceId={UserId}&token=&deviceType=3";

        return await _client.GetStringAsync<DeleteResponse>(endpoint);
    }

    public async Task<ErrorResponse> DeleteETicketForBookingServiceAsync(
        string baseUrl, string branchId, string number)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        if (string.IsNullOrEmpty(branchId) || string.IsNullOrEmpty(number))
            throw new ArgumentException("One or more required parameters are missing.");

        var endpoint = $"{baseUrl}/api/E_Ticket/DeleteTicket?branchId={branchId}&number={number}";

        return await _client.PostAsync<ErrorResponse>(endpoint);
    }
}
