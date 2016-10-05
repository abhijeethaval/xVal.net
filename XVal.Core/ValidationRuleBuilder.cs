using System;

namespace XVal.Core
{
    public class ValidationRuleBuilder<TEntity>
    {
        private Predicate<TEntity> _validateExpn;

        public ValidationRuleBuilder(Predicate<TEntity> validateExpn)
        {
            _validateExpn = validateExpn;
        }

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