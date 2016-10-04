using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace XVal.Core.Sample
{
    public class ValidationRuleBuilderSelector<TEntity>
    {
        public ChildValidationRuleBuilder<TEntity, TChild> ForChild<TChild>(Expression<Func<TEntity, TChild>> ChildExprn)
        {
            throw new NotImplementedException();
        }

        public ChildrenValidationRuleBuilder<TEntity, TChild> ForChildren<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> childrenExprn)
        {
            throw new NotImplementedException();
        }

        public ValidationRuleBuilder<TEntity> Validate(Predicate<TEntity> validateExpn)
        {
            throw new NotImplementedException();
        }

        public CompositeValidationRuleBuilder<TEntity> Validate(params IValidationRule<TEntity>[] childRules)
        {
            throw new NotImplementedException();
        }
    }
}