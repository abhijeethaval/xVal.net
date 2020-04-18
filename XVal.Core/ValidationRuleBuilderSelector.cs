using System;
using System.Collections.Generic;

namespace XVal.Core
{
    public class ValidationRuleBuilderSelector<TEntity>
    {
        internal ValidationRuleBuilderSelector()
        {

        }

        public ChildValidationRuleBuilder<TEntity, TChild> ForChild<TChild>(Func<TEntity, TChild> ChildExprn)
            => new ChildValidationRuleBuilder<TEntity, TChild>(ChildExprn);

        public CollectionChildValidationRuleBuilder<TEntity, TChild> ForChildren<TChild>(Func<TEntity, IEnumerable<TChild>> childrenExprn)
            => new CollectionChildValidationRuleBuilder<TEntity, TChild>(childrenExprn);

        public ValidationRuleBuilder<TEntity> Validate(Predicate<TEntity> validateExpn)
            => new ValidationRuleBuilder<TEntity>(validateExpn);

        public CompositeValidationRuleBuilder<TEntity> Validate(params IValidationRule<TEntity>[] childRules)
            => new CompositeValidationRuleBuilder<TEntity>(childRules);
    }
}