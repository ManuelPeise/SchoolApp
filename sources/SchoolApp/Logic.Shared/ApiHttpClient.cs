using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Settings;
using Logic.Shared.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Logic.Shared
{
    public class ApiHttpClient<TRequest, TResponse> : IApiHttpClient<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        private readonly ISettingsService _settingsService;

        public ApiHttpClient(IConfiguration configuration, ILocalDatabaseAccessor databaseAccessor, ISettingsService settingsService)
        {
            _databaseAccessor = databaseAccessor;
            _settingsService = settingsService;

            var baseAddress = GetBaseAddress();

            _httpClient = new HttpClient
            {
                BaseAddress = !string.IsNullOrEmpty(baseAddress) ?
                new Uri(baseAddress, UriKind.Absolute) : throw new ArgumentNullException(nameof(baseAddress)),
            };
        }

        public async Task<TResponse?> GetAsync(Uri url, List<KeyValuePair<string, object>> parameters, string? token = null)
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

                if (!string.IsNullOrWhiteSpace(token))
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

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
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {

                    Message = $"Get request for ${url} failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return null;
            }
        }

        public async Task<TResponse?> PostAsync(string url, TRequest? model, string? token = null)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(url))
                {
                    return null;
                }

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url, UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrWhiteSpace(token))
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

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
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {

                    Message = $"Post request for ${url} failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return null;
            }
        }

        public async Task<ResponseBaseModel> ApiIsReachable(string url = "api/availability/isavailable")
        {
            var reponseBase = new ResponseBaseModel
            {
                Success = false
            };

            try
            {
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    Version = HttpVersion.Version20,
                    RequestUri = new Uri($"{url}", UriKind.Relative)
                };

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    reponseBase.Success = true;
                }

                return await Task.FromResult(reponseBase);
            }
            catch (Exception)
            {
                return await Task.FromResult(reponseBase);
            }
        }

        public void SetBaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress, UriKind.Absolute);
        }

        private string GetBaseAddress()
        {
            var result = _settingsService.LoadApiSettings().Result;

            if (result == null)
            {
                throw new Exception("Api settings could not be loaded");
            }

            var baseAddress = !string.IsNullOrEmpty(result.ApiBaseUrl) && result.Port != null ?
                $"{result.ApiBaseUrl}:{result.Port}" : "https://localhost:7239";

            return baseAddress;
        }
    }
}
