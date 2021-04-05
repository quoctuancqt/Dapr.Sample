using System;

namespace SharedKernel.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base($"Record with id: {message} does not exist.")
        {
        }
    }
}
