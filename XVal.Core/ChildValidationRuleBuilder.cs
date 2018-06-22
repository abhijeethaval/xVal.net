using System;
using System.Linq;

namespace XVal.Core
{
    public class ChildValidationRuleBuilder<TEntity, TChild>
    {
        private readonly Func<TEntity, TChild> _childExprn;
        private IValidationRule<TChild> _childRule;
        private Predicate<TEntity> _precondition;
        private Func<TEntity, string> _messageFormatter;

        public ChildValidationRuleBuilder(Func<TEntity, TChild> childExprn)
        {
            _childExprn = childExprn;
        }

        public ChildValidationRuleBuilder<TEntity, TChild> Validate(IValidationRule<TChild> childRule)
        {
            _childRule = childRule;
            return this;
        }

        public ChildValidationRuleBuilder<TEntity, TChild> When(Predicate<TEntity> precondition)
        {
            _precondition = precondition;
            return this;
        }

        public ChildValidationRuleBuilder<TEntity, TChild> Message(
            string format,
            params Func<TEntity, object>[] formatParameters)
        {
            format.ThrowIfArgumentNullOrWhiteSpace(nameof(format));
            _messageFormatter = e => string.Format(format, formatParameters.Select(f => f(e)).ToArray());
            return this;
        }

        public ChildValidationRuleBuilder<TEntity, TChild> Message(
            Func<TEntity, string> messageFormatter)
        {
            _messageFormatter = messageFormatter;
            return this;
        }

        public ChildValidationRule<TEntity, TChild> Build()
        {
            return new ChildValidationRule<TEntity, TChild>(_precondition,
                _messageFormatter,
                _childExprn,
                _childRule);
        }

        public static implicit operator ChildValidationRule<TEntity, TChild>(ChildValidationRuleBuilder<TEntity, TChild> builder)
        {
            return builder.Build();
        }
    }
}