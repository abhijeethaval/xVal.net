﻿using System;
using System.Linq;

namespace XVal.Core
{
    public sealed class ValidationRuleBuilder<TEntity>
        : ValidationRuleBuilderBase<ValidationRuleBuilder<TEntity>, TEntity>
    {
        private readonly Predicate<TEntity> _validateExpn;

        internal ValidationRuleBuilder(Predicate<TEntity> validateExpn)
        {
            _validateExpn = validateExpn.Validate(nameof(validateExpn));
        }

        public ValidationRule<TEntity> Build()
        {
            return new ValidationRule<TEntity>(
                Precondition,
                MessageFormatter,
                new SimpleValidationStrategy<TEntity>(_validateExpn));
        }

        public static implicit operator ValidationRule<TEntity>(ValidationRuleBuilder<TEntity> builder)
        {
            return builder.Build();
        }
    }
}