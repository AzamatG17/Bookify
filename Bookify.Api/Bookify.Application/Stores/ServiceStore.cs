using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.Stores;
using Bookify.Application.Responses;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Stores;

internal sealed class ServiceStore : IServiceStore
{
    private readonly IApiClient _client;

    public ServiceStore(IApiClient apiClient)
    {
        _client = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<List<Service>> GetDataOnlinetAsync(Branch branch, List<string> languages)
    {
        if (string.IsNullOrEmpty(branch.Companies.BaseUrl))
            throw new ArgumentNullException(nameof(branch.Companies.BaseUrl));
        if (languages == null || languages.Count == 0)
            throw new ArgumentNullException(nameof(languages));

        var serviceMap = new Dictionary<int, Service>();

        foreach (var language in languages)
        {
            var endpoint = $"{branch.Companies.BaseUrl}/OnlinetBookingServiceRest/ListServices?BranchId={branch.BranchId}&languageShortId={language}";

            var serviceResponses = await _client.GetAsync<List<ServiceResponse>>(endpoint);

            if (serviceResponses != null && serviceResponses.Any())
            {
                MapToOnlinetService(serviceResponses, branch.Id, language, serviceMap);
            }
        }

        return serviceMap.Values.ToList();
    }

    public async Task<List<Service>> GetDataBookingServiceAsync(Branch branch, List<string> languages)
    {
        if (string.IsNullOrEmpty(branch.Companies.BaseUrl))
            throw new ArgumentNullException(nameof(branch.Companies.BaseUrl));
        if (languages == null || languages.Count == 0)
            throw new ArgumentNullException(nameof(languages));

        var serviceMap = new Dictionary<int, Service>();

        foreach (var language in languages)
        {
            var endpoint = $"{branch.Companies.BaseUrl}/api/ListServices?BranchId={branch.BranchId}&languageShortId={language}";

            var serviceResponses = await _client.GetAsync<List<ServiceResponse>>(endpoint);

            if (serviceResponses != null && serviceResponses.Any())
            {
                MapToService(serviceResponses, branch.Id, language, serviceMap);
            }
        }

        return serviceMap.Values.ToList();
    }

    public static List<Service> MapToService(List<ServiceResponse> serviceResponses, int branchId, string languageCode, Dictionary<int, Service> serviceMap)
    {
        foreach (var response in serviceResponses)
        {
            if (!serviceMap.TryGetValue(response.ServiceId, out var service))
            {
                service = new Service
                {
                    ServiceId = response.ServiceId,
                    BranchId = branchId,
                    ServiceTranslations = new List<ServiceTranslation>()
                };
                serviceMap[response.ServiceId] = service;
            }

            service.ServiceTranslations.Add(new ServiceTranslation
            {
                Name = response.ServiceName,
                LanguageCode = languageCode,
                ServiceId = response.ServiceId
            });
        }

        return serviceMap.Values.ToList();
    }

    public static List<Service> MapToOnlinetService(List<ServiceResponse> serviceResponses, int branchId, string languageCode, Dictionary<int, Service> serviceMap)
    {
        foreach (var response in serviceResponses)
        {
            if (!serviceMap.TryGetValue(response.ServiceId, out var service))
            {
                service = new Service
                {
                    ServiceId = response.ServiceId,
                    BranchId = branchId,
                    ServiceTranslations = new List<ServiceTranslation>()
                };
                serviceMap[response.ServiceId] = service;
            }

            service.ServiceTranslations.Add(new ServiceTranslation
            {
                Name = response.ServiceName,
                LanguageCode = languageCode,
                ServiceId = response.ServiceId
            });
        }

        return serviceMap.Values.ToList();
    }
}
