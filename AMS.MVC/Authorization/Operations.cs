using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AMS.MVC.Authorization
{
    public static class MovieOperations
    {
        public static readonly OperationAuthorizationRequirement Edit = new OperationAuthorizationRequirement
        {
            Name = Constants.EditOperation
        };
    }

    public static class MovieStarOperations
    {
        public static readonly OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement
        {
            Name = Constants.CreateOperation
        };
        
        public static readonly OperationAuthorizationRequirement Edit = new OperationAuthorizationRequirement
        {
            Name = Constants.EditOperation
        };
        
        public static readonly OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement
        {
            Name = Constants.DeleteOperation
        };
    }

    public static class Constants
    {
        public const string CreateOperation = "Create";
        public const string EditOperation = "Edit";
        public const string DeleteOperation = "Delete";
    }
}
