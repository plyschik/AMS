using System;

namespace AMS.Exceptions
{
    public class GenreAlreadyAttachedToMovie : Exception
    {
        public GenreAlreadyAttachedToMovie()
        {
        }

        public GenreAlreadyAttachedToMovie(string message) : base(message)
        {
        }
    }
}
