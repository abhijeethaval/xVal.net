using System;

namespace XVal.Core
{
    public class ChildValidationRule<TEntity, TChild> : IValidationRule<TEntity>
    {
        public ChildValidationRule(Predicate<TEntity> precondition,
            MessageFormatter<TEntity> messageFormatter,
            Func<TEntity, TChild> childExprn,
            IValidationRule<TChild> childValidationRule)
        {
            messageFormatter.ThrowIfArgumentNull(nameof(messageFormatter));
            childExprn.ThrowIfArgumentNull(nameof(childExprn));
            childValidationRule.ThrowIfArgumentNull(nameof(childValidationRule));
            Precondition = precondition;
            MessageFormatter = messageFormatter;
            ChildExprn = childExprn;
            InternalValidationRule = childValidationRule;
        }

        public Predicate<TEntity> Precondition { get; }
        public MessageFormatter<TEntity> MessageFormatter { get; }
        public Func<TEntity, TChild> ChildExprn { get; }
        public IValidationRule<TChild> InternalValidationRule { get; }

        public ValidationResult Execute(TEntity entity)
        {
            return ValidationRuleHelper.Validate(entity,
                Precondition,
                ExecuteHelper);
        }

        private ValidationResult ExecuteHelper(TEntity entity)
        {
            var child = ChildExprn.Invoke(entity);
            if (child == null) return ValidationResult.Passed();
            var childResult = InternalValidationRule.Execute(child);
            return childResult
                ? ValidationResult.Passed()
                : ValidationResult.Failed(MessageFormatter.GetMessage(entity) + Environment.NewLine + childResult.Message);

        }
    }
}