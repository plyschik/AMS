using System;

namespace AMS.Exceptions
{
    public class DirectorAlreadyAttachedToMovie : Exception
    {
        public DirectorAlreadyAttachedToMovie()
        {
        }

        public DirectorAlreadyAttachedToMovie(string message) : base(message)
        {
        }
    }
}
