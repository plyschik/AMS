using System;

namespace AMS.Exceptions
{
    public class PersonAlreadyExists : Exception
    {
        public PersonAlreadyExists()
        {
        }

        public PersonAlreadyExists(string message) : base(message)
        {
        }
    }
}
