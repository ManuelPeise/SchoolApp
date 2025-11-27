using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;

namespace Logic.Shared
{
    public abstract class ALogicBase: ILogicBase
    {
        private readonly CurrentUser? _currentUser;
        public CurrentUser? CurrentUser => _currentUser;
        
        public ALogicBase(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Claims ?? null;
           
            if (claims != null)
            {
                _currentUser = new CurrentUser
                {
                    UserId = int.Parse(claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "0"),
                    UserName = claims.FirstOrDefault(x => x.Type == "name")?.Value ?? string.Empty,
                    UserRole = Enum.TryParse<UserRoleEnum>(claims.FirstOrDefault(x => x.Type == "userRole")?.Value, out var role)
                        ? role
                        : UserRoleEnum.None
                };
            }
        }
    }
}
