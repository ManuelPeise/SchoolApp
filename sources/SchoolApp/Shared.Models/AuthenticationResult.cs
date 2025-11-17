namespace Shared.Models
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
