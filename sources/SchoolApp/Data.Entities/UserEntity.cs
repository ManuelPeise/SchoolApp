using Shared.Enums;

namespace Data.Entities
{
    public class UserEntity: AEntityBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
    }
}
