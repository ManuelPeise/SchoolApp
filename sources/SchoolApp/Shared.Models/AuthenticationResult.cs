using Data.Entities.User;

namespace Shared.Models
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public AppUserEntity User { get; set; } = new();
    }
}
