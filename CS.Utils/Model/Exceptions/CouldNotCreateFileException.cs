using System;

namespace ArsuLeo.CS.Utils.Model.Exceptions
{
    public class CouldNotCreateFileException : Exception
    {
        public string FullName { get; private set; }

        public CouldNotCreateFileException(string fullName, string message) : base(message)
        {
            FullName = fullName;
        }

        public CouldNotCreateFileException(string fullName, string message, Exception innerException) : base(message, innerException)
        {
            FullName = fullName;
        }

        public CouldNotCreateFileException()
        {
            FullName = string.Empty;
        }

        public CouldNotCreateFileException(string message) : base(message)
        {
            FullName = string.Empty;
        }

        public CouldNotCreateFileException(string message, Exception innerException) : base(message, innerException)
        {
            FullName = string.Empty;
        }
    }
}