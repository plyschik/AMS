using System;

namespace AMS.Exceptions
{
    public class UserNameAlreadyTaken : Exception
    {
        public UserNameAlreadyTaken()
        {
        }

        public UserNameAlreadyTaken(string message) : base(message)
        {
        }
    }
}
