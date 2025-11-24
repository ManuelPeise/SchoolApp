using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IApiHttpClient<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        Task<TResponse?> GetAsync(Uri url, List<KeyValuePair<string, object>> parameters, string? token = null);
        Task<TResponse?> PostAsync(string url, TRequest? model, string? token = null);
        Task<ResponseBaseModel> ApiIsReachable(string url = "api/availability/isavailable");
    }
}
