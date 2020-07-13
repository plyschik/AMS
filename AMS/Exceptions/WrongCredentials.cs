using System;

namespace AMS.Exceptions
{
    public class WrongCredentials : Exception
    {
        public WrongCredentials()
        {
        }

        public WrongCredentials(string message) : base(message)
        {
        }
    }
}
