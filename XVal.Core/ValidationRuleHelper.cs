using System;

namespace XVal.Core
{
    internal static class ValidationRuleHelper
    {
        internal static ValidationResult Validate<TEntity>(TEntity entity,
            Predicate<TEntity> precondition,
            Func<TEntity, ValidationResult> validator)
        {
            return precondition == null || precondition(entity)
                ? validator.Invoke(entity)
                : ValidationResult.Passed();
        }
    }
}
