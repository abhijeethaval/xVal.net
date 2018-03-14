using System;

namespace XVal.Core
{
    public class ChildValidationRuleBuilder<TEntity, TChild>
    {
        private readonly Func<TEntity, TChild> _childExprn;
        private IValidationRule<TChild> _childRule;
        private Predicate<TEntity> _precondition;
        private string _messageFormat;
        private Func<TEntity, object>[] _formatParameters;

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

        public ChildValidationRuleBuilder<TEntity, TChild> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            _messageFormat = format;
            _formatParameters = formatParameters;
            return this;
        }

        public ChildValidationRule<TEntity, TChild> Build()
        {
            return new ChildValidationRule<TEntity, TChild>(_precondition,
                new MessageFormatter<TEntity>(_messageFormat, _formatParameters),
                _childExprn,
                _childRule);
        }

        public static implicit operator ChildValidationRule<TEntity, TChild>(ChildValidationRuleBuilder<TEntity, TChild> builder)
        {
            return builder.Build();
        }
    }
}