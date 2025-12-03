namespace Shared.Models
{
    public class TokenStore
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireTime { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpireTime;
    }
}
