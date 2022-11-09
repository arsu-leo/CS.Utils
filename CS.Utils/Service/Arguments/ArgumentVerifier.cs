using System;
using System.Diagnostics.CodeAnalysis;

namespace ArsuLeo.CS.Utils.Service.Arguments
{
    /// <summary>
    /// Class which contains argument verification logic and which can throw exceptions if necessary. This code makes it easier to verify input arguments. 
    /// </summary>
    public static class ArgumentVerifier
    {
        /// <summary>
        /// Checks the argument passed in. if it's null, it throws an ArgumentNullException
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="paramName">The name.</param>
        public static void CantBeNull([AllowNull][NotNull] object argument, string paramName, string? message = null)
        {
            if (argument is null)
            {
                if (message is null)
                {
                    throw new ArgumentNullException(paramName);
                }
                else
                {
                    throw new ArgumentNullException(paramName, message);
                }
            }
        }


        /// <summary>
        /// Checks if the argument returns true with the func passed in. If not, it throws an ArgumentException with the error message specified. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checkFunc">The check func.</param>
        /// <param name="argument">The argument.</param>
        /// <param name="formattedError">The formatted error message.</param>
        public static void ShouldBeTrue<T>(Func<T, bool> checkFunc, T argument, string formattedError, string? paramName = null)
        {
            CantBeNull(checkFunc, nameof(checkFunc));
            if (!checkFunc(argument))
            {
                if (paramName is null)
                {
                    throw new ArgumentException(formattedError);
                }
                else
                {
                    throw new ArgumentException(formattedError, paramName);
                }
            }
        }
    }
}
