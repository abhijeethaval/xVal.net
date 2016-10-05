using System;

namespace XVal.Core
{
    public class CollectionChildValidationRuleBuilder<TEntity, TChild>
    {
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