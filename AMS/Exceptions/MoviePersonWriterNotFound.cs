using System;

namespace AMS.Exceptions
{
    public class MoviePersonWriterNotFound : Exception
    {
        public MoviePersonWriterNotFound()
        {
        }

        public MoviePersonWriterNotFound(string message) : base(message)
        {
        }
    }
}
