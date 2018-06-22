using System;
using System.Linq;

namespace XVal.Core
{
    public class ValidationRuleBuilderBase<TBuilder, TEntity>
        where TBuilder : ValidationRuleBuilderBase<TBuilder, TEntity>
    {
        protected Func<TEntity, string> MessageFormatter { get; private set; }
        protected Predicate<TEntity> Precondition { get; private set; }
        
        public TBuilder When(Predicate<TEntity> precondition)
        {
            Precondition = precondition;
            return (TBuilder)this;
        }

        public TBuilder Message(
            string format,
            params Func<TEntity, object>[] formatParameters)
        {
            format.ThrowIfArgumentNullOrWhiteSpace(nameof(format));
            MessageFormatter = e => string.Format(format, formatParameters.Select(f => f(e)).ToArray());
            return (TBuilder)this;
        }

        public TBuilder Message(
            Func<TEntity, string> messageFormatter)
        {
            MessageFormatter = messageFormatter;
            return (TBuilder)this;
        }
    }
}