using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    public class CompositeValidationRule<TEntity> : IValidationRule<TEntity>
    {
        internal CompositeValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
            IEnumerable<IValidationRule<TEntity>> childRules)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            childRules.ThrowIfArgumentNull(nameof(childRules));
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            ChildRules = childRules.Where(c => c != null);
        }

        public Predicate<TEntity> Precondition { get; }
        public IEnumerable<IValidationRule<TEntity>> ChildRules { get; }
        public MessageFormatter<TEntity> MessageFormatter { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity, Precondition, ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            if (!ChildRules.Any())
            {
                return ValidationResult.Passed();
            }

            var childResult = ChildRules.Select(c => c.Execute(entity)).Aggregate(ValidationResult.Combine);
            if (childResult)
            {
                return ValidationResult.Passed();
            }

            return ValidationResult.Failed(MessageFormatter.GetMessage(entity) + Environment.NewLine + childResult.Message);
        }
    }
}
