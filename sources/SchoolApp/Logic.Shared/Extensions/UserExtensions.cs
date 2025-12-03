using Data.Entities.User;
using Logic.Shared.Models;

namespace Logic.Shared.Extensions
{
    public static class UserExtensions
    {
        public static ObservableUser ToObservable(this AppUserEntity entity)
        {
            return new ObservableUser
            {
                Id = entity.Id,
                FamilyId = entity.FamilyId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                DateOfBirth = entity.DateOfBirth,
                Salt = entity.Credentials.Salt,
                Password = entity.Credentials.Password,
                UserRole = entity.UserRole,
                RefreshToken = entity.Credentials.RefreshToken,
                IsActive = entity.IsActive,
                IsInSync = entity.IsInSync,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
            };
        }

        public static AppUserEntity ToEntity(this ObservableUser user)
        {
            return new AppUserEntity
            {
                Id = user.Id,
                FamilyId = user.FamilyId,
                LastName = user.LastName,
                FirstName = user.UserName,
                DateOfBirth = user.DateOfBirth, 
                UserRole = user.UserRole,
                IsActive = user.IsActive,
                IsInSync = user.IsInSync,
                Credentials = new AppUserCredentialsEntity
                {
                    Salt = user.Salt,
                    Password = user.Password,
                    RefreshToken = user.RefreshToken
                },
                CreatedAt = user.CreatedAt,
                CreatedBy = user.CreatedBy,
                UpdatedAt = user.UpdatedAt,
                UpdatedBy = user.UpdatedBy
            };
        }
    }
}
