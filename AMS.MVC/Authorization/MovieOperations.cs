using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AMS.MVC.Authorization
{
    public class MovieOperations
    {
        public static OperationAuthorizationRequirement Edit = new OperationAuthorizationRequirement
        {
            Name = Constants.EditOperationName
        };

        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement
        {
            Name = Constants.DeleteOperationName
        };
    }
    
    public class Constants
    {
        public const string EditOperationName = "Edit";
        public const string DeleteOperationName = "Delete";
    }
}
