using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    internal class CollectionChildValidationStrategy<TEntity, TChild> : IValidationRule<TEntity>
    {
        private readonly Func<TEntity, IEnumerable<TChild>> _collection;
        private readonly IValidationRule<TChild> _childValidationRule;

        internal CollectionChildValidationStrategy(
            Func<TEntity, IEnumerable<TChild>> collection,
            IValidationRule<TChild> childValidationRule)
        {
            _collection = collection.Validate(nameof(collection));
            _childValidationRule = childValidationRule.Validate(nameof(childValidationRule));
        }

        public ValidationResult Execute(TEntity entity)
        {
            var collection = _collection.Invoke(entity);
            if (collection == null) return ValidationResult.Passed();
            var children = collection.ToList();
            if (!children.Any()) return ValidationResult.Passed();
            var executeItemsQuery = children.Select(item => _childValidationRule.Execute(item));
            return executeItemsQuery.Aggregate(ValidationResult.Combine);
        }
    }

    public class CollectionChildValidationRule<TEntity, TChild> : ValidationRule<TEntity>
    {
        internal CollectionChildValidationRule(Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
            Func<TEntity, IEnumerable<TChild>> collection,
            IValidationRule<TChild> childValidationRule)
        : base(precondition, messageFormatter, new CollectionChildValidationStrategy<TEntity, TChild>(collection, childValidationRule))
        {
        }
    }
}
