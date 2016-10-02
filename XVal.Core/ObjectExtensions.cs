using System;

namespace XVal.Core
{
    public static class ObjectExtensions
    {
        public static void ThrowIfArgumentNull(this object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
