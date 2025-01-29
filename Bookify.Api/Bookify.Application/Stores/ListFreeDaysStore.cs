using Bookify.Application.DTOs;
using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Responses;
using System.Globalization;

namespace Bookify.Application.Stores;

internal sealed class ListFreeDaysStore : IListFreeDaysStore
{
    private readonly IApiClient _client;

    public ListFreeDaysStore(IApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<List<FreeDayDto>> GetDataBookingServiceAsync(int branchId, int serviceId, DateTime startDate, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        string formattedDate = startDate.ToString("yyyy-MM-dd");

        var endpoint = $"{baseUrl}/api/ListFreeDaysMonth?branchId={branchId}&serviceId={serviceId}&startDate={formattedDate}";
        var freeDaysResponse = await _client.GetAsync<List<FreeDayResponse>>(endpoint);

        return MapToFreeDayDto(freeDaysResponse);
    }

    public async Task<List<FreeDayDto>> GetDataOnlinetAsync(int branchId, int serviceId, DateTime startDate, string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        string formattedDate = startDate.ToString("yyyyMMdd");

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/ListFreeDaysMonth?branchId={branchId}&serviceId={serviceId}&startDate={formattedDate}&languageShortId=ru";
        var freeDaysResponse = await _client.GetAsync<List<FreeDayResponse>>(endpoint);

        return MapToFreeDayDto(freeDaysResponse);
    }

    private static List<FreeDayDto> MapToFreeDayDto(List<FreeDayResponse> freeDayResponses)
    {
        return freeDayResponses.Select(d => new FreeDayDto(
            DateTime.ParseExact(d.Date, "yyyyMMdd", CultureInfo.InvariantCulture),
            d.Day,
            d.Month,
            d.Year,
            d.FreePlacesCount,
            d.TotalPlacesCount
        )).ToList();
    }
}
