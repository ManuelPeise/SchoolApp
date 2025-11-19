using Data.Entities.User;

namespace Logic.Shared.Models.Authentication
{
    public class LoginResult: ResponseModelBase
    {
        public string? JwtToken { get; set; }
        public AppUserEntity? AppUser { get; set; }
    }
}
