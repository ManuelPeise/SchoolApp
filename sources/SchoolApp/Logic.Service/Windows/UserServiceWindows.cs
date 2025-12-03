using Data.Entities.Administration;
using Logic.Service.Windows.Interfaces;
using Logic.Shared.Extensions;
using Logic.Shared.Models;
using Logic.Shared.Storage;
using Shared.Enums;

namespace Logic.Service.Windows
{
    public class UserServiceWindows: IUserServiceWindows
    {
        private int _currentUserId;
        private readonly ILocalDatabaseAccessor _localDatabaseAccessor;
        private ObservableUser? _currentUser;

        public ObservableUser? CurrentUser { get { return _currentUser; } }
        
        
        public UserServiceWindows(ILocalDatabaseAccessor localDatabaseAccessor)
        {
            _localDatabaseAccessor = localDatabaseAccessor;
        }

     
        public async Task<ObservableUser?> GetCurrentUser(string userIdString)
        {
            try
            {
                if(!int.TryParse(userIdString, out var userId))
                {
                    throw new ArgumentException(nameof(userIdString));
                }

                var userEntity = await _localDatabaseAccessor.UserRepository.GetByIdAsync(userId, true, x => x.Credentials);

                return userEntity?.ToObservable();
            }
            catch (Exception exception)
            {
                await _localDatabaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load user from local database",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _localDatabaseAccessor.SaveChangesAsync(null);
            }
        }

        public Task StoreUserData(string userId, string token)
        {
            Preferences.Set(PreferencesConstants.CurrentUserIdKey, userId);
        }
    }
}
