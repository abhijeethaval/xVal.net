using System;

namespace XVal.Core
{
    public static class StringExtensions
    {
        public static void ThrowIfArgumentNullOrWhiteSpace(this string argument, string argumentName)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException("Value cannot be empty string or white space.", argumentName);
            }
        }
    }
}
