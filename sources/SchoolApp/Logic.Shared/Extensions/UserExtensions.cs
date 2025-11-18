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
                Username = entity.Username,
                Salt = entity.Salt,
                Password = entity.Password,
                DateOfBirth = entity.DateOfBirth,
                UserRole = entity.UserRole
            };
        }

        public static AppUserEntity ToEntity(this ObservableUser user)
        {
            return new AppUserEntity
            {
                Username = user.Username,
                Salt = user.Salt,
                Password = user.Password,
                DateOfBirth = user.DateOfBirth,
                UserRole = user.UserRole
            };
        }
    }
}
