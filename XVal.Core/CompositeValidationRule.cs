using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    public class CompositeValidationRule<TEntity> : IValidationRule<TEntity>
    {
        public CompositeValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
            IEnumerable<IValidationRule<TEntity>> childRules)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            childRules.ThrowIfArgumentNull(nameof(childRules));
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            ChildRules = childRules;
        }

        public Predicate<TEntity> Precondition { get; }
        public IEnumerable<IValidationRule<TEntity>> ChildRules { get; }
        MessageFormatter<TEntity> MessageFormatter { get; }

        public ValidationResult Execute(TEntity entity)
        {
            if (Precondition.SatisfiedBy(entity))
            {
                var childResult = ChildRules.Select(c => c.Execute(entity)).Aggregate(ValidationResult.Combine);
                if (childResult)
                {
                    return ValidationResult.Passed();
                }

                return ValidationResult.Failed(MessageFormatter.GetMessage(entity) + Environment.NewLine + childResult.Message);
            }

            return ValidationResult.Passed();
        }
    }
}
