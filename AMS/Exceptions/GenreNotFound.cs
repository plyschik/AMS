using System;

namespace AMS.Exceptions
{
    public class GenreNotFound : Exception
    {
        public GenreNotFound()
        {
        }

        public GenreNotFound(string message) : base(message)
        {
        }
    }
}
