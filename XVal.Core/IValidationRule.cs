namespace XVal.Core
{
    public interface IValidationRule<in TEntity>
    {
        ValidationResult Execute(TEntity entity);
    }   
}
