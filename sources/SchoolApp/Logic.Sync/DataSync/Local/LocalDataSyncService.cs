using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;
using Shared.Models.Sync;
using Logic.Sync.DataSync.Extensions;
using Logic.Sync.Interfaces;

namespace Logic.Sync.DataSync.Local
{
    /// <summary>
    /// Provides local synchronization operations for application user profile data.
    /// Loads and persists <see cref="AppUserSyncModel"/> instances against the local database.
    /// </summary>
    public class LocalDataSyncService: ILocalDataSyncService
    {
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        private readonly ICurrentUserService _currentUserService;
        /// <summary>
        /// Initializes a new instance of <see cref="LocalDataSyncService"/>.
        /// </summary>
        /// <param name="databaseAccessor">Local database accessor used for repository operations.</param>
        /// <param name="currentUserService">Service used to determine the current user.</param>
        public LocalDataSyncService(ILocalDatabaseAccessor databaseAccessor, ICurrentUserService currentUserService)
        {
            _databaseAccessor = databaseAccessor;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Loads the current user's data from the local database and converts it to an <see cref="AppUserSyncModel"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="AppUserSyncModel"/> for the current user, or <c>null</c> if no current user is set or the user cannot be found.
        /// </returns>
        /// <exception cref="System.Exception">Logged and swallowed; any exception will result in <c>null</c> being returned.</exception>
        public async Task<AppUserSyncModel?> GetUserSyncModel()
        {
            try
            {
                await _currentUserService.SetCurrentUser();
                var userId = _currentUserService.GetCurrentUserId();

                if (userId == null)
                {
                    return null;
                }

                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync((int)userId, true, x => x.Credentials, x => x.Family);

                if (userEntity == null || userEntity.IsInSync)
                {
                    return null;
                }

                var userSyncModel = userEntity.ToSyncModel();

                return userSyncModel;
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load user sync model from local database",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);
                return null;
            }
        }

        /// <summary>
        /// Saves values from an <see cref="AppUserSyncModel"/> into the local user entity.
        /// Only updates existing user records; does not create new users.
        /// </summary>
        /// <param name="userSyncModel">The sync model containing updated user data.</param>
        /// <returns><c>true</c> when the save succeeded; otherwise <c>false</c>.</returns>
        /// <exception cref="System.Exception">Logged and swallowed; exceptions result in <c>false</c> being returned.</exception>
        public async Task<bool> SaveUserSyncModel(AppUserSyncModel userSyncModel)
        {
            if (userSyncModel is null)
            {
                throw new ArgumentNullException(nameof(userSyncModel));
            }
            try
            {
                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync(userSyncModel.Id, true, x => x.Credentials, x => x.Family);
                
                if (userEntity == null)
                {
                    return false;
                }

                // Update user entity properties from sync model
                userEntity.FirstName = userSyncModel.FirstName;
                userEntity.LastName = userSyncModel.LastName;
                userEntity.UserName = userSyncModel.UserName;
                userEntity.IsInSync = true;

                if (userEntity.Credentials != null && userSyncModel.Credentials != null)
                {
                    userEntity.Credentials.Salt = userSyncModel.Credentials.Salt;
                    userEntity.Credentials.Password = userSyncModel.Credentials.Password;
                    userEntity.Credentials.RefreshToken = userSyncModel.Credentials.RefreshToken;
                }
                
                _databaseAccessor.UserRepository.Update(userEntity);
                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return true;
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not save user sync model to local database",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return false;
            }
        }
    }
}
