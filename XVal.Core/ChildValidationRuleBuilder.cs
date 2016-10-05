using System;
using System.Linq.Expressions;

namespace XVal.Core
{
    public class ChildValidationRuleBuilder<TEntity, TChild>
    {
        private Expression<Func<TEntity, TChild>> _childExprn;

        public ChildValidationRuleBuilder(Expression<Func<TEntity, TChild>> childExprn)
        {
            _childExprn = childExprn;
        }

        public ChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> addressRule)
        {
            throw new NotImplementedException();
        }

        public ChildValidationRuleBuilder<TEntity, TChild> When(Predicate<TEntity> precondition)
        {
            throw new NotImplementedException();
        }

        public ChildValidationRuleBuilder<TEntity, TChild> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            throw new NotImplementedException();
        }

        public object Build()
        {
            throw new NotImplementedException();
        }
    }
}