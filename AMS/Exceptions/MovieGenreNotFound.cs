using System;

namespace AMS.Exceptions
{
    public class MovieGenreNotFound : Exception
    {
        public MovieGenreNotFound()
        {
        }

        public MovieGenreNotFound(string message) : base(message)
        {
        }
    }
}
