using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class JwtAuthAttribute : Attribute, IAuthorizationFilter
    {
        public string UserRoleString { get; set; } = string.Empty;
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRoles = GetUserRoles();

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

                if (userRoles.Contains(role))
                {
                    isAuthenticated = true;
                }
            }

            if (!isAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }

        private List<UserRoleEnum> GetUserRoles()
        {
            var userRoles = new List<UserRoleEnum>();

            if (!string.IsNullOrEmpty(UserRoleString))
            {
                var roles = UserRoleString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var role in roles)
                {
                    if (Enum.TryParse<UserRoleEnum>(role.Trim(), out var parsedRole))
                    {
                        userRoles.Add(parsedRole);
                    }
                }
            }

            return userRoles;
        }
    }
}
