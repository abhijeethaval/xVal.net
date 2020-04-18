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
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            Precondition = precondition;
            MessageFormatter = messageFormatter ?? throw new ArgumentNullException(nameof(messageFormatter));
        }

        public Predicate<TEntity> Precondition { get; }
        public Func<TEntity, string> MessageFormatter { get; set; }

        public ValidationResult Execute(TEntity entity)
        {
            if (!(Precondition is null) && !Precondition(entity))
            {
                return ValidationResult.Passed();
            }

            var result = _strategy.Execute(entity);
            if (result.Result)
            {
                return ValidationResult.Passed();
            }

            var message =
                $"{MessageFormatter(entity)}{(string.IsNullOrWhiteSpace(result.Message) ? string.Empty : $"{Environment.NewLine}{result.Message}")}";
            return ValidationResult.Failed(message);
        }
    }
}
