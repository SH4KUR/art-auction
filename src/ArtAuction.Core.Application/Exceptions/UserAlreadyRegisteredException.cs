using System;

namespace ArtAuction.Core.Application.Exceptions
{
    public class UserAlreadyRegisteredException : Exception
    {
        public UserAlreadyRegisteredException()
        {
        }

        public UserAlreadyRegisteredException(string message)
            : base(message)
        {
        }

        public UserAlreadyRegisteredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}