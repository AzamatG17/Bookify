﻿using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Stores;

internal sealed class BranchStore(IApiClient apiClient) : IBranchStore
{
    private readonly IApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

    public async Task<List<Branch>> GetAllAsync(Companies companies)
    {
        if (string.IsNullOrEmpty(companies.BaseUrlForBookingService))
            throw new ArgumentNullException(nameof(companies.BaseUrlForBookingService));

        var endpoint = $"{companies.BaseUrlForBookingService}/api/Branches/ListBranches";
        var branches = await _apiClient.GetStringAsync<List<NewBranchResponse>>(endpoint);

        return MapToBranch(branches, companies.Id);
    }

    public async Task<List<Branch>> GetAllForOnlinetAsync(Companies companies)
    {
        if (string.IsNullOrEmpty(companies.BaseUrlForOnlinet))
            throw new ArgumentNullException(nameof(companies.BaseUrlForOnlinet));

        var endpoint = $"{companies.BaseUrlForOnlinet}/OnlinetBookingServiceRest/ListBranches";
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
            Projects = Domain_.Enums.Projects.BookingService,
            OpeningTimeBranches = r.BranchOpenHours?.Select(o =>
                {
                    TimeSpan startTime = TimeSpan.Zero;
                    TimeSpan endTime = TimeSpan.Zero;

                    if (!string.IsNullOrWhiteSpace(o.OpenTime))
                    {
                        var parts = o.OpenTime.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2)
                        {
                            TimeSpan.TryParse(parts[0], out startTime);
                            TimeSpan.TryParse(parts[1], out endTime);
                        }
                    }

                    return new OpeningTimeBranch
                    {
                        Day = o.Day,
                        OpenTime = o.OpenTime,
                        StartTime = startTime,
                        EndTime = endTime
                    };
                }).ToList() ?? [],
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
            Projects = Domain_.Enums.Projects.Onlinet,
            OpeningTimeBranches = r.BranchOpeningTime?.Select(o => 
                {
                    TimeSpan startTime = TimeSpan.Zero;
                    TimeSpan endTime = TimeSpan.Zero;

                    if (!string.IsNullOrWhiteSpace(o.OpenTime))
                    {
                        var parts = o.OpenTime.Split(" - ", StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2)
                        {
                            TimeSpan.TryParse(parts[0], out startTime);
                            TimeSpan.TryParse(parts[1], out endTime);
                        }
                    }

                    return new OpeningTimeBranch
                    {
                        Day = o.Day,
                        OpenTime = o.OpenTime,
                        StartTime = startTime,
                        EndTime = endTime
                    };
                }).ToList() ?? [],
            CompanyId = companyId
        }).ToList();
    }
}
