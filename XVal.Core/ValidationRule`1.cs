using System;

namespace XVal.Core
{
    public class ValidationRule<TEntity> : IValidationRule<TEntity>
    {
        internal ValidationRule(
            Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
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
        public Func<TEntity, string> MessageFormatter { get; set; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity, Precondition, ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            var result = ValidateExprn(entity);
            if (!result)
            {
                return ValidationResult.Failed(MessageFormatter.Invoke(entity));
            }

            return ValidationResult.Passed();
        }
    }
}
