using System;

namespace Eventsuffle.Core.Exceptions
{
    public class InvalidISODateException : Exception
    {
        public InvalidISODateException()
        {

        }

        public InvalidISODateException(string invalidDateString)
            : base($"Invalid date: {invalidDateString}")
        {

        }
    }
}
