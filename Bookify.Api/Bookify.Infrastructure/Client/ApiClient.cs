using Bookify.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Bookify.Infrastructure.Client;

internal sealed class ApiClient : IApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ApiClient> _logger;

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

        var result = JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result is null)
        {
            _logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method GET",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
    }

    public async Task<TResult> PostAsync<TResult, TRequest>(string url, TRequest request)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResult>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result is null)
        {
            _logger.LogWarning(
                "Response deserialization returned null for type {Type} from resource {Url} and method POST",
                typeof(TResult),
                url);

            throw new InvalidCastException("Could not deserialize response");
        }

        return result;
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
