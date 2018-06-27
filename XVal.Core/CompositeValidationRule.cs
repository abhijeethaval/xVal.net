using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    internal class CompositeValidationStrategy<TEntity> : IValidationRule<TEntity>
    {
        private readonly List<IValidationRule<TEntity>> _childRules;

        internal CompositeValidationStrategy(IEnumerable<IValidationRule<TEntity>> childRules)
        {
            _childRules = childRules.Validate(nameof(childRules)).ToList();
        }

        public ValidationResult Execute(TEntity entity)
        {
            var childRules = _childRules.Where(c => c != null).ToList();
            return childRules.Any()
                ? childRules.Select(c => c.Execute(entity)).Aggregate(ValidationResult.Combine)
                : ValidationResult.Passed();
        }
    }

    public class CompositeValidationRule<TEntity> : ValidationRule<TEntity>
    {
        internal CompositeValidationRule(
            Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
            IEnumerable<IValidationRule<TEntity>> childRules)
        : base(precondition, messageFormatter, new CompositeValidationStrategy<TEntity>(childRules))
        {
        }
    }
}
