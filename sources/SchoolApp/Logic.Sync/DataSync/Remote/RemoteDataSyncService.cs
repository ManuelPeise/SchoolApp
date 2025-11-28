using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared;
using Logic.Shared.Storage;
using Logic.Sync.DataSync.Extensions;
using Logic.Sync.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Sync;

namespace Logic.Sync.DataSync.Remote
{
    public class RemoteDataSyncService: ALogicBase, IRemoteDataSyncService
    {
        private readonly IRemoteDatabaseAccessor _databaseAccessor;

        public RemoteDataSyncService(IHttpContextAccessor httpContextAccessor, IRemoteDatabaseAccessor remoteDatabaseAccessor): base(httpContextAccessor)
        {
            _databaseAccessor = remoteDatabaseAccessor;
        }

        /// <summary>
        /// Updates an <see cref="AppUserEntity"/> in the remote database from the provided <see cref="AppUserSyncModel"/>.
        /// The method will load the target entity, map values from the sync model and persist the changes.
        /// </summary>
        /// <param name="syncModel">The sync model containing updated values for the user.</param>
        /// <returns>
        /// The updated <see cref="AppUserSyncModel"/> reflecting the stored state, or <c>null</c> if the user was not found or an error occurred.
        /// </returns>
        /// <exception cref="System.Exception">Exceptions are logged; method returns <c>null</c> on error.</exception>
        public async Task<AppUserSyncModel?> UpdateAppUserEntity(AppUserSyncModel syncModel)
        {
            try
            {
                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync(syncModel.Id);

                if (userEntity == null)
                {
                    await _databaseAccessor.LogMessage(new LogEntryEntity
                    {
                        Message = $"User with ID {syncModel.Id} not found in remote database.",
                        LogLevel = LogLevelEnum.Warning,
                    });

                    await _databaseAccessor.SaveChangesAsync();

                    return null;
                }

                var syncEntity = syncModel.ToEntity();

                MapAppUserEntitys(userEntity, syncEntity);

                _databaseAccessor.UserRepository.Update(userEntity);

                await _databaseAccessor.SaveChangesAsync();

                return userEntity.ToSyncModel();
            }
            catch(Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Unexpected error while updating user entity in remote database",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error,
                });

                await _databaseAccessor.SaveChangesAsync();

                return null;
            }
        }

        /// <summary>
        /// Maps values from a sync entity to the original entity instance.
        /// Only updates existing related entities (family, credentials) if they are already loaded on the original entity.
        /// This method does not create new related entities.
        /// </summary>
        /// <param name="originalEntity">The existing entity loaded from the database to be updated.</param>
        /// <param name="syncEntity">The sync entity containing new values to apply.</param>
        private void MapAppUserEntitys(AppUserEntity originalEntity, AppUserEntity syncEntity)
        {
            originalEntity.FirstName = syncEntity.FirstName;
            originalEntity.LastName = syncEntity.LastName;
            originalEntity.UserName = syncEntity.UserName;
            originalEntity.DateOfBirth = syncEntity.DateOfBirth;
            originalEntity.UserRole = syncEntity.UserRole;
            originalEntity.IsActive = syncEntity.IsActive;
            originalEntity.IsInSync = syncEntity.IsInSync;
            originalEntity.LastSyncAt = DateTime.UtcNow;

            if (syncEntity.Family != null)
            {
                originalEntity.FamilyId = syncEntity.Family.Id;

                if (originalEntity.Family != null)
                {
                    originalEntity.Family.FamilyDisplayName = syncEntity.Family.FamilyDisplayName;
                    originalEntity.Family.FamilyName = syncEntity.Family.FamilyName;
                    originalEntity.Family.IsInSync = syncEntity.Family.IsInSync;
                    originalEntity.Family.CreatedAt = syncEntity.Family.CreatedAt;
                    originalEntity.Family.CreatedBy = syncEntity.Family.CreatedBy;
                    originalEntity.Family.UpdatedAt = syncEntity.Family.UpdatedAt;
                    originalEntity.Family.UpdatedBy = syncEntity.Family.UpdatedBy;
                    originalEntity.Family.LastSyncAt = DateTime.UtcNow;
                }
            }
            else if (syncEntity.FamilyId.HasValue)
            {
                originalEntity.FamilyId = syncEntity.FamilyId;
            }

            if (syncEntity.Credentials != null)
            {
                originalEntity.CredentialsId = syncEntity.Credentials.Id;

                if (originalEntity.Credentials != null)
                {
                    originalEntity.Credentials.Salt = syncEntity.Credentials.Salt;
                    originalEntity.Credentials.Password = syncEntity.Credentials.Password;
                    originalEntity.Credentials.RefreshToken = syncEntity.Credentials.RefreshToken;
                    originalEntity.Credentials.IsInSync = syncEntity.Credentials.IsInSync;
                    originalEntity.Credentials.LastSyncAt = syncEntity.Credentials.LastSyncAt;
                }
            }
        }
    }
}
