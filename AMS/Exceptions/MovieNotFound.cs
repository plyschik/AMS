using System;

namespace AMS.Exceptions
{
    public class MovieNotFound : Exception
    {
        public MovieNotFound()
        {
        }

        public MovieNotFound(string message) : base(message)
        {
        }
    }
}
