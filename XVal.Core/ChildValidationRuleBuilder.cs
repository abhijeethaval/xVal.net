using System;

namespace XVal.Core
{
    public sealed class ChildValidationRuleBuilder<TEntity, TChild>
        : ValidationRuleBuilderBase<ChildValidationRuleBuilder<TEntity, TChild>, TEntity>
    {
        private readonly Func<TEntity, TChild> _childExprn;
        private IValidationRule<TChild>? _childRule;

        internal ChildValidationRuleBuilder(Func<TEntity, TChild> childExprn) =>
            _childExprn = childExprn;

        public ChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> childRule)
        {
            _childRule = childRule;
            return this;
        }

        public ChildValidationRule<TEntity, TChild> Build()
        {
            if (_childRule is null)
            {
                throw new InvalidOperationException("Cannot build without child rule. Please provide child rule by calling Validate");
            }

            if (MessageFormatter is null)
            {
                throw new InvalidOperationException("Cannot build without message. Please provide message or message formatter by calling Message");
            }

            return new ChildValidationRule<TEntity, TChild>(
                Precondition,
                MessageFormatter,
                _childExprn,
                _childRule);
        }

        public static implicit operator ChildValidationRule<TEntity, TChild>(ChildValidationRuleBuilder<TEntity, TChild> builder) =>
            builder.Build();
    }
}