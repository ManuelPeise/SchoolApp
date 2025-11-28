using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Shared.Enums;
using Shared.Models;

namespace Logic.Shared.Storage
{
    public class DbStorageHandler<TRequest> : IDbStorageHandler<TRequest> where TRequest : class
    {
        private readonly IApiHttpClient<TRequest, ResponseBaseModel> _apiHealthClient;
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        public DbStorageHandler(ILocalDatabaseAccessor databaseAccessor, IApiHttpClient<TRequest, ResponseBaseModel> apiHealthClient)
        {
            _apiHealthClient = apiHealthClient;
            _databaseAccessor = databaseAccessor;
        }

        public async Task<bool> StoreData(
            TRequest model,
            string currentUser,
            Func<TRequest, Task<ResponseBaseModel>> mySqlUpdateFunction,
            Func<TRequest, string, bool, Task> sqLiteUpdateFunction)
        {
            try
            {
                var isStoredInMySql = false;
                var apiIsReachable = await IsApiAvailabe();

                if (apiIsReachable.Success)
                {
                    var response = await mySqlUpdateFunction(model);

                    isStoredInMySql = response?.Success ?? false;
                }

                await sqLiteUpdateFunction(model, currentUser, isStoredInMySql);

                return await Task.FromResult(true);
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    LogLevel = LogLevelEnum.Error,
                    Message = $"Error while store data [{typeof(TRequest).Name}].",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    IsInSync = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                });

                await _databaseAccessor.SaveChangesAsync(currentUser);

                return await Task.FromResult(false);
            }
        }

        private async Task<ResponseBaseModel> IsApiAvailabe()
        {
            return await _apiHealthClient.ApiIsReachable();
        }
    }
}
