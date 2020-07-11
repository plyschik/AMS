using System;

namespace AMS.Exceptions
{
    public class WrongCredentials : Exception
    {
        public WrongCredentials(string message) : base(message)
        {
        }
    }
}
