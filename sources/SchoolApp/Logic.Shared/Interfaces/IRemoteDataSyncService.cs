using System.Threading.Tasks;
using Shared.Models.Sync;

namespace Logic.Shared.Interfaces
{
    public interface IRemoteDataSyncService
    {
        /// <summary>
        /// Updates the remote <see cref="Data.Entities.User.AppUserEntity"/> using values from the provided <see cref="AppUserSyncModel"/>.
        /// The implementation should load the target entity, apply changes and persist them.
        /// </summary>
        /// <param name="syncModel">The sync model containing updated values for the user.</param>
        /// <returns>
        /// The updated <see cref="AppUserSyncModel"/> reflecting the stored state, or <c>null</c> if the user was not found or an error occurred.
        /// </returns>
        Task<AppUserSyncModel?> UpdateAppUserEntity(AppUserSyncModel syncModel);
    }
}
