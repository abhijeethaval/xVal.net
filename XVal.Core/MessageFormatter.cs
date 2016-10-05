using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    public class MessageFormatter<TEntity>
    {
        public MessageFormatter(string format, params Func<TEntity, object>[] arguments)
        {
            format.ThrowIfArgumentNullOrWhiteSpace(nameof(format));
            Format = format;
            Arguments = arguments;
        }

        public string Format { get; }

        public IEnumerable<Func<TEntity, object>> Arguments { get; }

        public string GetMessage(TEntity entity)
        {
            var arguments = from argument in Arguments select argument(entity);
            return string.Format(Format, arguments.ToArray());
        }
    }
}
