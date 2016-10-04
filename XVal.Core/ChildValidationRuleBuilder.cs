using System;

namespace XVal.Core.Sample
{
    public class ChildValidationRuleBuilder<TEntity, TChild>
    {
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