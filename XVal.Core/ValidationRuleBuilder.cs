using System;

namespace XVal.Core
{
    public sealed class ValidationRuleBuilder<TEntity>
        : ValidationRuleBuilderBase<ValidationRuleBuilder<TEntity>, TEntity>
    {
        private readonly Predicate<TEntity> _validateExpn;

        internal ValidationRuleBuilder(Predicate<TEntity> validateExpn)
            => _validateExpn = validateExpn ?? throw new ArgumentNullException(nameof(validateExpn));

        public ValidationRule<TEntity> Build()
        {
            if (MessageFormatter is null)
            {
                throw new InvalidOperationException("Cannot build without message. Please provide message or message formatter by calling Message");
            }

            return new ValidationRule<TEntity>(
                Precondition,
                MessageFormatter,
                new SimpleValidationStrategy<TEntity>(_validateExpn));
        }

        public static implicit operator ValidationRule<TEntity>(ValidationRuleBuilder<TEntity> builder)
            => builder.Build();
    }
}