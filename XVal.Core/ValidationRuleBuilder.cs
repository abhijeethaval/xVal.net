using System;
using System.Linq;

namespace XVal.Core
{
    public class ValidationRuleBuilder<TEntity>
    {
        private readonly Predicate<TEntity> _validateExpn;
        private Predicate<TEntity> _precondition;
        private Func<TEntity, string> _messageFormatter;


        public ValidationRuleBuilder(Predicate<TEntity> validateExpn)
        {
            validateExpn.ThrowIfArgumentNull(nameof(validateExpn));
            _validateExpn = validateExpn;
        }

        public ValidationRuleBuilder<TEntity> When(Predicate<TEntity> precondition)
        {
            _precondition = precondition;
            return this;
        }

        public ValidationRuleBuilder<TEntity> Message(
             string format,
             params Func<TEntity, object>[] formatParameters)
        {
            format.ThrowIfArgumentNullOrWhiteSpace(nameof(format));
            _messageFormatter = e => string.Format(format, formatParameters.Select(f => f(e)).ToArray());
            return this;
        }

        public ValidationRuleBuilder<TEntity> Message(Func<TEntity, string> messageFormatter)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            _messageFormatter = messageFormatter;
            return this;
        }

        public ValidationRule<TEntity> Build()
        {
            return new ValidationRule<TEntity>(
                _precondition,
                _messageFormatter,
                _validateExpn);
        }

        public static implicit operator ValidationRule<TEntity>(ValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}