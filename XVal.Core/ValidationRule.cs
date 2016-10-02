using System;

namespace XVal.Core
{
    public class ValidationRule<TEntity> : IValidationRule<TEntity>
    {
        public ValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
            Predicate<TEntity> validateExprn)
        {
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            ValidateExprn = validateExprn;
        }

        public Predicate<TEntity> Precondition { get; }
        public Predicate<TEntity> ValidateExprn { get; }
        MessageFormatter<TEntity> MessageFormatter { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return Precondition == null || !Precondition(entity) || ValidateExprn(entity)
                ? ValidationResult.Passed()
                : ValidationResult.Failed(MessageFormatter.GetMessage(entity));
        }
    }
}
