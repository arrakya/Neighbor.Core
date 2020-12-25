using System;

namespace Neighbor.Core.Domain.Exceptions
{
    public class RefershTokenInvalidException : Exception
    {
        public RefershTokenInvalidException(string message) : base(message)
        {

        }
    }
}
