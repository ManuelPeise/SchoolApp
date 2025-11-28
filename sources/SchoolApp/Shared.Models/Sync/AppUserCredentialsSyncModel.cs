namespace Shared.Models.Sync
{
    public class AppUserCredentialsSyncModel: AsyncModelBase
    {
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
