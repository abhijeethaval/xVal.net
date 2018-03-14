using System;

namespace XVal.Core
{
    public class ValidationRuleBuilder<TEntity>
    {
        private readonly Predicate<TEntity> _validateExpn;
        private Predicate<TEntity> _precondition;
        private Func<TEntity, object>[] _formatParameters;
        private string _format;

        public ValidationRuleBuilder(Predicate<TEntity> validateExpn)
        {
            _validateExpn = validateExpn;
        }

        public ValidationRuleBuilder<TEntity> When(Predicate<TEntity> precondition)
        {
            _precondition = precondition;
            return this;
        }

        public ValidationRuleBuilder<TEntity> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            _format = format;
            _formatParameters = formatParameters;
            return this;
        }

        public ValidationRule<TEntity> Build()
        {
            return new ValidationRule<TEntity>(_precondition,
                new MessageFormatter<TEntity>(_format, _formatParameters),
                _validateExpn);
        }

        public static implicit operator ValidationRule<TEntity>(ValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}