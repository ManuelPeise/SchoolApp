using Shared.Enums;

namespace Shared.Models
{
    public class UserModel
    {
        public string Username { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Salt { get; set; }
        public string? Password { get; set; }
        public UserRoleEnum UserRole { get; set; } = UserRoleEnum.User;
    }
}
