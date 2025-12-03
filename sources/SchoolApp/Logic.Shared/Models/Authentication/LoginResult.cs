using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Models.Authentication
{
    public class LoginResult: ResponseBaseModel
    {
        public string? JwtToken { get; set; }
        public AppUserEntity? AppUser { get; set; }
    }
}
