using System;

namespace AMS.Exceptions
{
    public class MoviePersonStarNotFound : Exception
    {
        public MoviePersonStarNotFound()
        {
        }

        public MoviePersonStarNotFound(string message) : base(message)
        {
        }
    }
}
