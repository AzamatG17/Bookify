using Bookify.Application.Interfaces;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Models;

namespace Bookify.Application.Stores;

internal sealed class BranchStore(IApiClient apiClient) : IBranchStore
{
    private readonly IApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

    public async Task<List<Branches>> GetAllAsync(string baseUrl)
    {
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var endpoint = $"{baseUrl}/api/Branches/ListBranches";
        return await _apiClient.GetAsync<List<Branches>>(endpoint);
    }
}
