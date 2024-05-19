using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Notes.WebAPI.CustomAttributes
{
    public class RolesAuthorizeAttribute : AuthorizeAttribute
    {
        public RolesAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }

    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            if (requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
