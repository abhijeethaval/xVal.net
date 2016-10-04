using System;

namespace XVal.Core.Sample
{
    public class ValidationRuleBuilder<TEntity>
    {
        public ValidationRuleBuilder<TEntity> When(Predicate<TEntity> precondition)
        {
            throw new NotImplementedException();
        }

        public ValidationRuleBuilder<TEntity> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            throw new NotImplementedException();
        }

        public ValidationRule<TEntity> Build()
        {
            return null;
        }

        public static implicit operator ValidationRule<TEntity>(ValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}