using System;

namespace AMS.Exceptions
{
    public class WriterAlreadyAttachedToMovie : Exception
    {
        public WriterAlreadyAttachedToMovie()
        {
        }

        public WriterAlreadyAttachedToMovie(string message) : base(message)
        {
        }
    }
}
