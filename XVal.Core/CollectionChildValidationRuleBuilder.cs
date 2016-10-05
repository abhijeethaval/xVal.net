using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace XVal.Core
{
    public class CollectionChildValidationRuleBuilder<TEntity, TChild>
    {
        private Expression<Func<TEntity, IEnumerable<TChild>>> _childrenExprn;

        public CollectionChildValidationRuleBuilder(Expression<Func<TEntity, IEnumerable<TChild>>> childrenExprn)
        {
            _childrenExprn = childrenExprn;
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> addressRule)
        {
            throw new NotImplementedException();
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> When(Predicate<TEntity> precondition)
        {
            throw new NotImplementedException();
        }

        public CollectionChildValidationRuleBuilder<TEntity, TChild> Message(string format,
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