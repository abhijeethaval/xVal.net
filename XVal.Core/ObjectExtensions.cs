using System;

namespace XVal.Core
{
    public static class ObjectExtensions
    {
        public static T Validate<T>(this T argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return argument;
        }
    }
}
