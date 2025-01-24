using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Stores;

internal sealed class BranchStore(IApiClient apiClient) : IBranchStore
{
    private readonly IApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

    public async Task<List<Branch>> GetAllAsync(Companies companies)
    {
        if (string.IsNullOrEmpty(companies.BaseUrl))
            throw new ArgumentNullException(nameof(companies.BaseUrl));

        var endpoint = $"{companies.BaseUrl}/api/Branches/ListBranches";
        var branches = await _apiClient.GetAsync<List<NewBranchResponse>>(endpoint);

        return MapToBranch(branches, companies.Id);
    }

    public async Task<List<Branch>> GetAllForOnlinetAsync(Companies companies)
    {
        if (string.IsNullOrEmpty(companies.BaseUrl))
            throw new ArgumentNullException(nameof(companies.BaseUrl));

        var endpoint = $"{companies.BaseUrl}/OnlinetBookingServiceRest/ListBranches";
        var branches = await _apiClient.GetAsync<List<BranchResponse>>(endpoint);

        return MapToOnlinetBranch(branches, companies.Id);
    }

    private static List<Branch> MapToBranch(List<NewBranchResponse> responses, int companyId)
    {
        return responses.Select(r => new Branch
        {
            BranchId = r.BranchId,
            Name = r.BranchName,
            BranchAddres = r.BranchAddress,
            CoordinateLatitude = r.BranchCoordinates.Latitude,
            CoordinateLongitude = r.BranchCoordinates.Longitude,
            OpeningTimeBranches = r.BranchOpenHours?.Select(o => new OpeningTimeBranch
            {
                Day = o.Day,
                OpenTime = o.OpenTime
            }).ToList() ?? new List<OpeningTimeBranch>(),
            CompanyId = companyId
        }).ToList();
    }

    private static List<Branch> MapToOnlinetBranch(List<BranchResponse> responses, int companyId)
    {
        return responses.Select(r => new Branch
        {
            BranchId = r.BranchId,
            Name = r.BranchName,
            BranchAddres = r.BranchAddress,
            CoordinateLatitude = r.BranchCoordinates.Latitude,
            CoordinateLongitude = r.BranchCoordinates.Longitude,
            OpeningTimeBranches = r.BranchOpeningTime?.Select(o => new OpeningTimeBranch
            {
                Day = o.Day,
                OpenTime = o.OpenTime
            }).ToList() ?? new List<OpeningTimeBranch>(),
            CompanyId = companyId
        }).ToList();
    }
}
