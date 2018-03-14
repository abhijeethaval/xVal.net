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
        {
            return new ChildValidationRuleBuilder<TEntity, TChild>(ChildExprn);
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> ForChildren<TChild>(Func<TEntity, IEnumerable<TChild>> childrenExprn)
        {
            return new CollectionChildValidationRuleBuilder<TEntity, TChild>(childrenExprn);
        }

        public ValidationRuleBuilder<TEntity> Validate(Predicate<TEntity> validateExpn)
        {
            return new ValidationRuleBuilder<TEntity>(validateExpn);
        }

        public CompositeValidationRuleBuilder<TEntity> Validate(params IValidationRule<TEntity>[] childRules)
        {
            return new CompositeValidationRuleBuilder<TEntity>(childRules);
        }
    }
}