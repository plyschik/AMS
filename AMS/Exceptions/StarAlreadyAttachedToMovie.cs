using System;

namespace AMS.Exceptions
{
    public class StarAlreadyAttachedToMovie : Exception
    {
        public StarAlreadyAttachedToMovie()
        {
        }

        public StarAlreadyAttachedToMovie(string message) : base(message)
        {
        }
    }
}
