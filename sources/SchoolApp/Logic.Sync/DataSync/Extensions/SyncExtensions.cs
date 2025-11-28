

using Data.Entities.User;
using Shared.Models.Sync;

namespace Logic.Sync.DataSync.Extensions
{
    public  static class SyncExtensions
    {
        /// <summary>
        /// Converts an <see cref="AppUserEntity"/> to its corresponding <see cref="AppUserSyncModel"/> representation.
        /// </summary>
        /// <param name="entity">The user entity to convert.</param>
        /// <returns>A new <see cref="AppUserSyncModel"/> populated from the given entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is <c>null</c>.</exception>
        public static AppUserSyncModel ToSyncModel(this AppUserEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            AppUserCredentialsSyncModel? credentials = null;

            if (entity.Credentials != null)
            {
                credentials = new AppUserCredentialsSyncModel
                {
                    Id = entity.Credentials.Id,
                    IsInSync = entity.Credentials.IsInSync,
                    CreatedAt = entity.Credentials.CreatedAt,
                    CreatedBy = entity.Credentials.CreatedBy,
                    UpdatedAt = entity.Credentials.UpdatedAt,
                    UpdatedBy = entity.Credentials.UpdatedBy,
                    LastSyncAt = entity.Credentials.LastSyncAt,
                    Salt = entity.Credentials.Salt,
                    Password = entity.Credentials.Password,
                    RefreshToken = entity.Credentials.RefreshToken,
                };
            }

            return new AppUserSyncModel
            {
                Id = entity.Id,
                FamilyId = entity.FamilyId,
                Family = entity.Family != null
                    ? new FamilySyncModel
                    {
                        Id = entity.Family.Id,
                        IsInSync = entity.Family.IsInSync,
                        CreatedAt = entity.Family.CreatedAt,
                        CreatedBy = entity.Family.CreatedBy,
                        UpdatedAt = entity.Family.UpdatedAt,
                        UpdatedBy = entity.Family.UpdatedBy,
                        LastSyncAt = entity.Family.LastSyncAt,
                        FamilyDisplayName = entity.Family.FamilyDisplayName,
                        FamilyName = entity.Family.FamilyName,
                    }
                    : null,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                UserName = entity.UserName,
                DateOfBirth = entity.DateOfBirth,
                UserRole = entity.UserRole,
                IsActive = entity.IsActive,
                CredentialsId = entity.CredentialsId,
                Credentials = credentials ?? new AppUserCredentialsSyncModel { Id = entity.CredentialsId },
                IsInSync = entity.IsInSync,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,
                LastSyncAt = entity.LastSyncAt,
            };
        }

        /// <summary>
        /// Converts an <see cref="AppUserSyncModel"/> back to an <see cref="AppUserEntity"/> instance.
        /// </summary>
        /// <param name="model">The sync model to convert.</param>
        /// <returns>A new <see cref="AppUserEntity"/> populated from the given sync model.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <c>null</c>.</exception>
        public static AppUserEntity ToEntity(this AppUserSyncModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AppUserCredentialsEntity? credentials = null;

            if (model.Credentials is not null)
            {
                credentials = new AppUserCredentialsEntity
                {
                    Id = model.Credentials.Id,
                    IsInSync = model.Credentials.IsInSync,
                    CreatedAt = model.Credentials.CreatedAt,
                    CreatedBy = model.Credentials.CreatedBy,
                    UpdatedAt = model.Credentials.UpdatedAt,
                    UpdatedBy = model.Credentials.UpdatedBy,
                    LastSyncAt = model.Credentials.LastSyncAt,
                    Salt = model.Credentials.Salt,
                    Password = model.Credentials.Password,
                    RefreshToken = model.Credentials.RefreshToken,
                };
            }

            return new AppUserEntity
            {
                Id = model.Id,
                FamilyId = model.FamilyId,
                Family = model.Family != null
                    ? new FamilyEntity
                    {
                        Id = model.Family.Id,
                        IsInSync = model.Family.IsInSync,
                        CreatedAt = model.Family.CreatedAt,
                        CreatedBy = model.Family.CreatedBy,
                        UpdatedAt = model.Family.UpdatedAt,
                        UpdatedBy = model.Family.UpdatedBy,
                        LastSyncAt = model.Family.LastSyncAt,
                        FamilyDisplayName = model.Family.FamilyDisplayName,
                        FamilyName = model.Family.FamilyName,
                    }
                    : (model.FamilyId.HasValue ? new FamilyEntity { Id = model.FamilyId.Value } : null),
                LastName = model.LastName,
                FirstName = model.FirstName,
                UserName = model.UserName,
                DateOfBirth = model.DateOfBirth,
                UserRole = model.UserRole,
                IsActive = model.IsActive,
                CredentialsId = model.CredentialsId,
                Credentials = credentials ?? new AppUserCredentialsEntity { Id = model.CredentialsId },
                IsInSync = model.IsInSync,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy,
                LastSyncAt = model.LastSyncAt,
            };
        }
    }
}
