using System;
using System.Collections.Generic;

namespace XVal.Core
{
    public class CollectionChildValidationRuleBuilder<TEntity, TChild>
    {
        private readonly Func<TEntity, IEnumerable<TChild>> _childrenExprn;
        private IValidationRule<TChild> _childRule;
        private Predicate<TEntity> _precondition;
        private string _messageFormat;
        private Func<TEntity, object>[] _formatParameters;

        public CollectionChildValidationRuleBuilder(Func<TEntity, IEnumerable<TChild>> childrenExprn)
        {
            _childrenExprn = childrenExprn;
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> childRule)
        {
            _childRule = childRule;
            return this;
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> When(Predicate<TEntity> precondition)
        {
            _precondition = precondition;
            return this;
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            _messageFormat = format;
            _formatParameters = formatParameters;
            return this;
        }

        public CollectionChildValidationRule<TEntity, TChild> Build()
        {
            return new CollectionChildValidationRule<TEntity, TChild>(_precondition,
                new MessageFormatter<TEntity>(_messageFormat, _formatParameters),
                _childrenExprn,
                _childRule);
        }

        public static implicit operator CollectionChildValidationRule<TEntity, TChild>(CollectionChildValidationRuleBuilder<TEntity, TChild> builder)
        {
            return builder.Build();
        }
    }
}