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
                LastName = entity.LastName,
                Username = entity.Username,
                DateOfBirth = entity.DateOfBirth,
                Salt = entity.Salt,
                Password = entity.Password,
                UserRole = entity.UserRole,
                RefreshToken = entity.RefreshToken,
                IsActive = entity.IsActive,
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
                Username = user.Username,
                DateOfBirth = user.DateOfBirth,
                Salt = user.Salt,
                Password = user.Password,
                UserRole = user.UserRole,
                RefreshToken = user.RefreshToken,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                CreatedBy = user.CreatedBy,
                UpdatedAt = user.UpdatedAt,
                UpdatedBy = user.UpdatedBy
            };
        }
    }
}
