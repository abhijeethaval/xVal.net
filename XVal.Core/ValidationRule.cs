using System;

namespace XVal.Core
{
    public class ValidationRule<TEntity> : IValidationRule<TEntity>
    {
        public ValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
            Predicate<TEntity> validateExprn)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            validateExprn.ThrowIfArgumentNull(nameof(validateExprn));
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            ValidateExprn = validateExprn;
        }

        public Predicate<TEntity> Precondition { get; }
        public Predicate<TEntity> ValidateExprn { get; }
        MessageFormatter<TEntity> MessageFormatter { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity, Precondition, ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            var result = ValidateExprn(entity);
            if (!result)
            {
                return ValidationResult.Failed(MessageFormatter.GetMessage(entity));
            }

            return ValidationResult.Passed();
        }
    }
}
