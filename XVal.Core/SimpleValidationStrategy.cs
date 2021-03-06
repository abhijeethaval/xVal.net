﻿using System;

namespace XVal.Core
{
    internal class SimpleValidationStrategy<TEntity> : IValidationRule<TEntity>
    {
        private readonly Predicate<TEntity> _validateExprn;

        internal SimpleValidationStrategy(Predicate<TEntity> validateExprn) => _validateExprn = validateExprn;

        public ValidationResult Execute(TEntity entity) => _validateExprn(entity) ? ValidationResult.Passed() : ValidationResult.Failed(string.Empty);
    }
}