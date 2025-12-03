namespace Data.Entities.User
{
    public class AppUserCredentialsEntity: AEntityBase
    {
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
