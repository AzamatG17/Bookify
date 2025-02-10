namespace Bookify.Application.Interfaces;

public interface IApiClient
{
    Task<TResult> GetAsync<TResult>(string url);
    Task<TResult> PostAsync<TResult, TRequest>(string url, TRequest request);
    Task PutAsync<TRequest>(string url, TRequest request);
    Task DeleteAsync(string url);
    Task<TResult> PostsAsync<TBody, TResult>(string url, TBody data);
    Task<TResult> PostAsync<TResult>(string url);
}
