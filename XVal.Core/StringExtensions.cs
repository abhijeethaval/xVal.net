using System;

namespace XVal.Core
{
    public static class StringExtensions
    {
        public static void ThrowIfArgumentNullOrWhiteSpace(this string argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException("Argument " + argumentName + " is empty string.");
            }
        }
    }
}
