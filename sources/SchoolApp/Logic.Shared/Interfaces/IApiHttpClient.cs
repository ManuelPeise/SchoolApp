namespace Logic.Shared.Interfaces
{
    public interface IApiHttpClient<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        Task<TResponse?> GetAsync(Uri url, List<KeyValuePair<string, object>> parameters);
        Task<TResponse?> PostAsync(string url, TRequest? model);
    }
}
