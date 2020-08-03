using System.Threading.Tasks;
using AMS.MVC.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AMS.MVC.Authorization
{
    public class MovieIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Movie>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MovieIsOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

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
            
            if (resource.UserId == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);   
            }

            return Task.CompletedTask;
        }
    }
}
