using Shared.Enums;

namespace Shared.Models
{
    public class CurrentUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
    }
}
