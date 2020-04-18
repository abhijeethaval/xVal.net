using System;
using System.Collections.Generic;

namespace XVal.Core
{
    public sealed class CompositeValidationRuleBuilder<TEntity> : ValidationRuleBuilderBase<CompositeValidationRuleBuilder<TEntity>, TEntity>
    {
        private readonly IEnumerable<IValidationRule<TEntity>> _childRules;

        internal CompositeValidationRuleBuilder(IEnumerable<IValidationRule<TEntity>> childRules)
            => _childRules = childRules;

        public CompositeValidationRule<TEntity> Build()
        {
            if (MessageFormatter is null)
            {
                throw new InvalidOperationException("Cannot build without message. Please provide message or message formatter by calling Message");
            }

            return new CompositeValidationRule<TEntity>(
                Precondition,
                MessageFormatter,
                _childRules);
        }

        public static implicit operator CompositeValidationRule<TEntity>(CompositeValidationRuleBuilder<TEntity> builder) => builder.Build();
    }
}