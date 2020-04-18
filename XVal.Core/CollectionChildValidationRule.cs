using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    internal class CollectionChildValidationStrategy<TEntity, TChild> : IValidationRule<TEntity>
    {
        private readonly Func<TEntity, IEnumerable<TChild>> _collectionExpression;
        private readonly IValidationRule<TChild> _childValidationRule;

        internal CollectionChildValidationStrategy(
            Func<TEntity, IEnumerable<TChild>> collection,
            IValidationRule<TChild> childValidationRule)
        {
            _collectionExpression = collection ?? throw new ArgumentNullException(nameof(collection));
            _childValidationRule = childValidationRule ?? throw new ArgumentNullException(nameof(childValidationRule));
        }

        public ValidationResult Execute(TEntity entity)
        {
            var collection = _collectionExpression.Invoke(entity);
            return collection == null
                ? ValidationResult.Passed()
                : collection
                .Select(item => _childValidationRule.Execute(item))
                .Aggregate(ValidationResult.Passed(), ValidationResult.Combine);
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
