using System;
using System.Collections.Generic;
using System.Linq;

namespace XVal.Core
{
    public class CollectionChildValidationRule<TEntity, TChild> : IValidationRule<TEntity>
    {
        public CollectionChildValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
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
        public MessageFormatter<TEntity> MessageFormatter { get; }
        public Func<TEntity, IEnumerable<TChild>> Collection { get; }
        public IValidationRule<TChild> ChildValidationRule { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity, Precondition, ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            var collection = Collection.Invoke(entity);
            if (collection != null)
            {
                var executeItemsQuery = from item in collection
                                        select ChildValidationRule.Execute(item);

                var result = executeItemsQuery.Aggregate(ValidationResult.Combine);
                return result
                    ? ValidationResult.Passed()
                    : ValidationResult.Failed(MessageFormatter.GetMessage(entity) + Environment.NewLine + result.Message);
            }

            return ValidationResult.Passed();
        }
    }
}
