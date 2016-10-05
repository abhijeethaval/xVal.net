using System;

namespace XVal.Core
{

    public class CompositeValidationRuleBuilder<TEntity>
    {
        private IValidationRule<TEntity>[] _childRules;

        public CompositeValidationRuleBuilder(IValidationRule<TEntity>[] childRules)
        {
            _childRules = childRules;
        }

        public CompositeValidationRuleBuilder<TEntity> When(Predicate<TEntity> precondition)
        {
            throw new NotImplementedException();
        }

        public CompositeValidationRuleBuilder<TEntity> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            throw new NotImplementedException();
        }

        public CompositeValidationRule<TEntity> Build()
        {
            return null;
        }

        public static implicit operator CompositeValidationRule<TEntity>(CompositeValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}