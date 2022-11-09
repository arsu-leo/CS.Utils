using System;

namespace ArsuLeo.CS.Utils.Model.Exceptions
{
    public class CouldNotCreateDirectoryException : Exception
    {
        public string FullName { get; private set; }

        public CouldNotCreateDirectoryException(string fullName, string message) : base(message)
        {
            FullName = fullName;
        }

        public CouldNotCreateDirectoryException(string fullName, string message, Exception innerException) : base(message, innerException)
        {
            FullName = fullName;
        }

        public CouldNotCreateDirectoryException() : base()
        {
            FullName = string.Empty;
        }

        public CouldNotCreateDirectoryException(string message) : base(message)
        {
            FullName = string.Empty;
        }

        public CouldNotCreateDirectoryException(string message, Exception innerException) : base(message, innerException)
        {
            FullName = string.Empty;
        }
    }
}