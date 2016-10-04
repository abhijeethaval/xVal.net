using System;

namespace XVal.Core.Sample
{
    public class ChildrenValidationRuleBuilder<TEntity, TChild>
    {
        public ChildrenValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> addressRule)
        {
            throw new NotImplementedException();
        }

        public ChildrenValidationRuleBuilder<TEntity, TChild> When(Predicate<TEntity> precondition)
        {
            throw new NotImplementedException();
        }

        public ChildrenValidationRuleBuilder<TEntity, TChild> Message(string format,
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