﻿using System.Collections.Generic;

namespace XVal.Core
{
    public sealed class CompositeValidationRuleBuilder<TEntity> : ValidationRuleBuilderBase<CompositeValidationRuleBuilder<TEntity>, TEntity>
    {
        private readonly IEnumerable<IValidationRule<TEntity>> _childRules;

        internal CompositeValidationRuleBuilder(IEnumerable<IValidationRule<TEntity>> childRules) 
            => _childRules = childRules;

        public CompositeValidationRule<TEntity> Build() => new CompositeValidationRule<TEntity>(
                Precondition,
                MessageFormatter,
                _childRules);

        public static implicit operator CompositeValidationRule<TEntity>(CompositeValidationRuleBuilder<TEntity> builder) => builder.Build();
    }
}