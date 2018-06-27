using System;

namespace XVal.Core
{
    public class ValidationRule<TEntity> : IValidationRule<TEntity>
    {
        private readonly IValidationRule<TEntity> _strategy;

        internal ValidationRule(
            Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
            IValidationRule<TEntity> strategy)
        {
            _strategy = strategy.Validate(nameof(strategy));
            Precondition = precondition;
            MessageFormatter = messageFormatter.Validate(nameof(messageFormatter));
        }

        public Predicate<TEntity> Precondition { get; }
        public Func<TEntity, string> MessageFormatter { get; set; }

        public ValidationResult Execute(TEntity entity)
        {
            if (Precondition != null && !Precondition(entity))
            {
                return ValidationResult.Passed();
            }

            var result = _strategy.Execute(entity);
            if (result.Result)
            {
                return ValidationResult.Passed();
            }

            var message =
                $"{MessageFormatter(entity)}{(string.IsNullOrWhiteSpace(result.Message) ? null : $"{Environment.NewLine}{result.Message}")}";
            return ValidationResult.Failed(message);
        }
    }
}
