using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AMS.MVC.Authorization
{
    public static class MovieOperations
    {
        public static readonly OperationAuthorizationRequirement Edit = new OperationAuthorizationRequirement
        {
            Name = Constants.EditOperationName
        };
    }
    
    public static class Constants
    {
        public const string EditOperationName = "Edit";
    }
}
