using System.Collections.Generic;

namespace XVal.Core.Tests
{
    public static class GenericExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }
    }
}
