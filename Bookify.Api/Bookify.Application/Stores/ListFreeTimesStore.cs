using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Responses;

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

    public async Task<List<FreeTimeDto>> GetDataOnlinetAsync(int branchId, int serviceId, DateTime startDate, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        string formattedDate = startDate.ToString("yyyyMMdd");

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/ListFreeTimes?branchId={branchId}&serviceId={serviceId}&startDate={formattedDate}&languageShortId=ru";

        var freetimes = await _client.GetAsync<List<FreeTimeOnlinetResponse>>(endpoint);

        return MapToFreeTimeOnlinetDto(freetimes);
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

    private static List<FreeTimeDto> MapToFreeTimeOnlinetDto(List<FreeTimeOnlinetResponse> freeTimeResponse)
    {
        return freeTimeResponse.Select(f => new FreeTimeDto(
            f.Time,
            true,
            f.FreePlacesCount,
            f.TotalPlacesCount
            )).ToList();
    }
}
