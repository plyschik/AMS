using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AMS.MVC.Authorization
{
    public class MovieAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Movie>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Movie resource
        )
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }
            
            if (requirement.Name != Constants.EditOperationName && requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }
            
            if (context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
