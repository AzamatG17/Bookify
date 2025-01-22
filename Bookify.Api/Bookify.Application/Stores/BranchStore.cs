using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Models;
using Bookify.Application.Responses;
using System.Linq;

namespace Bookify.Application.Stores;

internal sealed class BranchStore(IApiClient apiClient) : IBranchStore
{
    private readonly IApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

    public async Task<List<Branches>> GetAllAsync(string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Branches/ListBranches";
        var branches = await _apiClient.GetAsync<List<NewBranchResponse>>(endpoint);

        return MapBranches(branches);
    }

    public async Task<List<Branches>> GetAllForOnlinetAsync(string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/OnlinetBookingServiceRest/ListBranches";
        var branches = await _apiClient.GetAsync<List<BranchResponse>>(endpoint);

        return MapBranchesOnlinet(branches);
    }

    public static List<Branches> MapBranchesOnlinet(List<BranchResponse> responses)
    {
        return responses.Select(br => new Branches(
            br.BranchId,
            br.BranchName,
            br.BranchAddress,
            br.BranchCoordinates != null
                ? new Coordinates(br.BranchCoordinates.Latitude, br.BranchCoordinates.Longitude)
                : null,
            br.BranchOpeningTime != null
                ? br.BranchOpeningTime.Select(ot => new OpeningTimeDto(ot.Day, ot.OpenTime)).ToList()
                : new List<OpeningTimeDto>()
        )).ToList();
    }

    public static List<Branches> MapBranches(List<NewBranchResponse> responses)
    {
        return responses.Select(br => new Branches(
            br.BranchId,
            br.BranchName,
            br.BranchAddress,
            br.BranchCoordinates != null
                ? new Coordinates(br.BranchCoordinates.Latitude, br.BranchCoordinates.Longitude)
                : null,
            br.BranchOpenHours != null
                ? br.BranchOpenHours.Select(ot => new OpeningTimeDto(ot.Day, ot.OpenTime)).ToList()
                : new List<OpeningTimeDto>()
        )).ToList();
    }
}
