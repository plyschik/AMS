using System;

namespace AMS.Exceptions
{
    public class PersonNotFound : Exception
    {
        public PersonNotFound()
        {
        }

        public PersonNotFound(string message) : base(message)
        {
        }
    }
}
