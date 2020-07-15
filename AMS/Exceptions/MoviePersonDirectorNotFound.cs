using System;

namespace AMS.Exceptions
{
    public class MoviePersonDirectorNotFound : Exception
    {
        public MoviePersonDirectorNotFound()
        {
        }

        public MoviePersonDirectorNotFound(string message) : base(message)
        {
        }
    }
}
