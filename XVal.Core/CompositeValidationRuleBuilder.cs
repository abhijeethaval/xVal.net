using System;
using System.Collections.Generic;

namespace XVal.Core
{
    public class CompositeValidationRuleBuilder<TEntity>
    {
        private IEnumerable<IValidationRule<TEntity>> _childRules;
        private Predicate<TEntity> _precondition;
        private string _format;
        private Func<TEntity, object>[] _formatParameters;

        public CompositeValidationRuleBuilder(IEnumerable<IValidationRule<TEntity>> childRules)
        {
            _childRules = childRules;
        }

        public CompositeValidationRuleBuilder<TEntity> When(Predicate<TEntity> precondition)
        {
            _precondition = precondition;
            return this;
        }

        public CompositeValidationRuleBuilder<TEntity> Message(string format,
            params Func<TEntity, object>[] formatParameters)
        {
            _format = format;
            _formatParameters = formatParameters;
            return this;
        }

        public CompositeValidationRule<TEntity> Build()
        {
            return new CompositeValidationRule<TEntity>(_precondition,
                new MessageFormatter<TEntity>(_format, _formatParameters),
                _childRules);
        }

        public static implicit operator CompositeValidationRule<TEntity>(CompositeValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}