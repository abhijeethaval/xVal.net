using System;
using System.Collections.Generic;

namespace XVal.Core
{
    public sealed class CollectionChildValidationRuleBuilder<TEntity, TChild> : ValidationRuleBuilderBase<CollectionChildValidationRuleBuilder<TEntity, TChild>, TEntity>
    {
        private readonly Func<TEntity, IEnumerable<TChild>> _childrenExprn;
        private IValidationRule<TChild>? _childRule;

        internal CollectionChildValidationRuleBuilder(Func<TEntity, IEnumerable<TChild>> childrenExprn)
            => _childrenExprn = childrenExprn;

        public CollectionChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> childRule)
        {
            _childRule = childRule;
            return this;
        }

        public CollectionChildValidationRule<TEntity, TChild> Build()
        {
            if (_childRule is null)
            {
                throw new InvalidOperationException("Cannot build without child rule. Please provide child rule by calling Validate");
            }

            if (MessageFormatter is null)
            {
                throw new InvalidOperationException("Cannot build without message. Please provide message or message formatter by calling Message");
            }

            return new CollectionChildValidationRule<TEntity, TChild>(
                Precondition,
                MessageFormatter,
                _childrenExprn,
                _childRule);
        }

        public static implicit operator CollectionChildValidationRule<TEntity, TChild>(CollectionChildValidationRuleBuilder<TEntity, TChild> builder) => builder.Build();
    }
}