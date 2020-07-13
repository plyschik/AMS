using System;

namespace AMS.Exceptions
{
    public class GenreAlreadyExists : Exception
    {
        public GenreAlreadyExists()
        {
        }

        public GenreAlreadyExists(string message) : base(message)
        {
        }
    }
}
