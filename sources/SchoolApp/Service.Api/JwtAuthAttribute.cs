using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class JwtAuthAttribute : Attribute, IAuthorizationFilter
    {
        public UserRoleEnum UserRole { get; set; } = UserRoleEnum.None;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticated = false;

            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new ForbidResult();

                return;
            }

            var roleClaimValue = user.Claims.FirstOrDefault(x => x.Type == "userRole")?.Value ?? null;

            if (!string.IsNullOrEmpty(roleClaimValue))
            {
                var role = (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), roleClaimValue);

                if (role == UserRole)
                {
                    isAuthenticated = true;
                }
                else
                {
                    isAuthenticated = UserRole == role;
                }
            }

            if (!isAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
