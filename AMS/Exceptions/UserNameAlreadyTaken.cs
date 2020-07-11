using System;

namespace AMS.Exceptions
{
    public class UserNameAlreadyTaken : Exception
    {
        public UserNameAlreadyTaken(string message) : base(message)
        {
        }
    }
}
