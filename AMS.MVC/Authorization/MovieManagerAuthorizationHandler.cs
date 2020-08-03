using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AMS.MVC.Authorization
{
    public class MovieManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Movie>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Movie resource
        )
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.EditOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("Manager"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
