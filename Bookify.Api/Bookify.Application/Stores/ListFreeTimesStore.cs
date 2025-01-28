using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Stores;

internal sealed class ListFreeTimesStore : IListFreeTimesStore
{
    private readonly IApiClient _client;

    public ListFreeTimesStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<List<FreeTimeDto>> GetDataBookingServiceAsync(int branchId, int serviceId, DateTime startDate, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        string formattedDate = startDate.ToString("yyyy-MM-dd");

        var endpoint = $"{baseUrl}/api/ListFreeTimes?branchId={branchId}&serviceId={serviceId}&startDate={formattedDate}";
        var freetimes = await _client.GetAsync<List<FreeTimeResponse>>(endpoint);

        return MapToFreeTimeDto(freetimes);
    }

    public Task GetDataOnlinetAsync(int branchId, int serviceId, DateTime startDate)
    {
        throw new NotImplementedException();
    }

    private static List<FreeTimeDto> MapToFreeTimeDto(List<FreeTimeResponse> freeTimeResponse)
    {
        return freeTimeResponse.Select(f => new FreeTimeDto(
            f.Time,
            f.IsAvailableTime,
            f.FreePlacesCount,
            f.TotalPlacesCount
            )).ToList();
    }
}
