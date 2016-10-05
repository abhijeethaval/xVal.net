namespace XVal.Core
{
    public static class ValidationRule
    {
        public static ValidationRuleBuilderSelector<TEntity> For<TEntity>()
        {
            return new ValidationRuleBuilderSelector<TEntity>();
        }
    }
}