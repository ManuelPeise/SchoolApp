using Shared.Models.Sync;

namespace Logic.Sync.Interfaces
{
    public interface ILocalDataSyncService
    {
        /// <summary>
        /// Loads the current user's data from the local database and converts it to an <see cref="AppUserSyncModel"/>.
        /// </summary>
        /// <returns>The <see cref="AppUserSyncModel"/> for the current user, or <c>null</c> if not available.</returns>
        Task<AppUserSyncModel?> GetUserSyncModel();

        /// <summary>
        /// Saves values from an <see cref="AppUserSyncModel"/> into the local user entity.
        /// </summary>
        /// <param name="userSyncModel">The sync model containing updated user data.</param>
        /// <returns><c>true</c> when the save succeeded; otherwise <c>false</c>.</returns>
        Task<bool> SaveUserSyncModel(AppUserSyncModel userSyncModel);
    }
}
