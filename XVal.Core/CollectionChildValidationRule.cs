using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    public class CollectionChildValidationRule<TEntity, TChild> : IValidationRule<TEntity>
    {
        internal CollectionChildValidationRule(Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
            Func<TEntity, IEnumerable<TChild>> collection,
            IValidationRule<TChild> childValidationRule)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            collection.ThrowIfArgumentNull(nameof(collection));
            childValidationRule.ThrowIfArgumentNull(nameof(childValidationRule));
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            Collection = collection;
            ChildValidationRule = childValidationRule;
        }

        public Predicate<TEntity> Precondition { get; }
        public Func<TEntity, string> MessageFormatter { get; }
        public Func<TEntity, IEnumerable<TChild>> Collection { get; }
        public IValidationRule<TChild> ChildValidationRule { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity, Precondition, ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            var collection = Collection.Invoke(entity);
            if (collection == null) return ValidationResult.Passed();
            var children = collection.ToList();
            if (!children.Any()) return ValidationResult.Passed();
            var executeItemsQuery = children.Select(item => ChildValidationRule.Execute(item));
            var result = executeItemsQuery.Aggregate(ValidationResult.Combine);
            return result
                ? ValidationResult.Passed()
                : ValidationResult.Failed(MessageFormatter.Invoke(entity) + Environment.NewLine + result.Message);
        }
    }
}
