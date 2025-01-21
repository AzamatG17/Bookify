using Bookify.Application.Interfaces;
using System.Text.Json;

namespace Bookify.Infrastructure.Client;

internal sealed class ApiClient : IApiClient
{
    private readonly HttpClient _client;

    public ApiClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));

    }

    public async Task<TResult> GetAsync<TResult>(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));

        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Deserialization returned null.");
    }

    public Task<TResult> PostAsync<TResult, TRequest>(string url, TRequest request)
    {
        throw new NotImplementedException();
    }

    public Task PutAsync<TRequest>(string url, TRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string url)
    {
        throw new NotImplementedException();
    }
}
