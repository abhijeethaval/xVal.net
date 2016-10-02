namespace XVal.Core
{
    public interface IValidationRule<TEntity>
    {
        ValidationResult Execute(TEntity entity);
    }   
}
