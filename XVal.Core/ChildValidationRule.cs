using System;

namespace XVal.Core
{
    internal class ChildValidationStrategy<TEntity, TChild> : IValidationRule<TEntity>
    {
        private readonly Func<TEntity, TChild> _childExprn;
        private readonly IValidationRule<TChild> _childValidationRule;

        internal ChildValidationStrategy(
            Func<TEntity, TChild> childExprn,
            IValidationRule<TChild> childValidationRule)
        {
            _childExprn = childExprn ?? throw new ArgumentNullException(nameof(childExprn));
            _childValidationRule = childValidationRule ?? throw new ArgumentNullException(nameof(childValidationRule));
        }

        public ValidationResult Execute(TEntity entity)
        {
            var child = _childExprn(entity);
            return child == null ? ValidationResult.Passed() : _childValidationRule.Execute(child);
        }
    }

    public class ChildValidationRule<TEntity, TChild> : ValidationRule<TEntity>
    {
        internal ChildValidationRule(Predicate<TEntity> precondition,
            Func<TEntity, string> messageFormatter,
            Func<TEntity, TChild> childExprn,
            IValidationRule<TChild> childValidationRule)
        : base(precondition, messageFormatter,
            new ChildValidationStrategy<TEntity, TChild>(childExprn, childValidationRule))
        {

        }
    }
}