using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    internal class CompositeValidationStrategy<TEntity> : IValidationRule<TEntity>
    {
        private readonly IEnumerable<IValidationRule<TEntity>> _childRules;

        internal CompositeValidationStrategy(IEnumerable<IValidationRule<TEntity>> childRules)
            => _childRules = (childRules ?? throw new ArgumentNullException(nameof(childRules))).ToList();

        public ValidationResult Execute(TEntity entity)
            => _childRules
            .Where(c => c != null)
            .Select(c => c.Execute(entity))
            .Aggregate(ValidationResult.Passed(), ValidationResult.Combine);
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
