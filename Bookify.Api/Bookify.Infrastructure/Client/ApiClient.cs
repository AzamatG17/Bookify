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

        try
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new InvalidOperationException("Deserialization returned null.");
        }
        catch (HttpRequestException httpEx)
        {
            throw new InvalidOperationException("An error occurred while making the HTTP request.", httpEx);
        }
        catch (JsonException jsonEx)
        {
            throw new InvalidOperationException("Failed to deserialize the response content.", jsonEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred.", ex);
        }
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
