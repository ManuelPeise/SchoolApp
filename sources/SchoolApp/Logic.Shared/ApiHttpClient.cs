using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Enums;
using System.Net;
using System.Text;

namespace Logic.Shared
{
    public class ApiHttpClient<TRequest, TResponse> : IApiHttpClient<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILogService _logService;

        public ApiHttpClient(IConfiguration configuration, ILogService logService)
        {
            var apiBaseAddress = configuration.GetValue<string>("ApiBaseUrl");
            _logService = logService;

            _httpClient = new HttpClient
            {
                BaseAddress = !string.IsNullOrEmpty(apiBaseAddress) ?
                new Uri(apiBaseAddress) : throw new ArgumentNullException(nameof(apiBaseAddress)),
            };
        }

        public async Task<TResponse?> GetAsync(Uri url, List<KeyValuePair<string, object>> parameters)
        {
            try
            {
                var urlParameters = $"?${string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    Version = HttpVersion.Version20,
                    RequestUri = new Uri($"{url}{urlParameters}")
                };

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(content);
                }

                return null;
            }
            catch (Exception exception)
            {
                await _logService.LogMessageSqLite(new LogEntryEntity
                {

                    Message = $"Get request for ${url} failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return null;
            }
        }

        public async Task<TResponse?> PostAsync(string url, TRequest? model)
        {
            try
            {
                if(model == null || string.IsNullOrEmpty(url))
                {
                    return null;
                }

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Version = HttpVersion.Version20,
                    RequestUri = new Uri(url, UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(content);
                }

                return null;
            }
            catch (Exception exception)
            {
                await _logService.LogMessageSqLite(new LogEntryEntity
                {

                    Message = $"Post request for ${url} failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return null;
            }
        }
    }
}
