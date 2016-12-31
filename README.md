# What is it?
xVal.net is a fluent validation library for .NET. It is designed to be flexible.
It allows specifying the validation rules for the .NET objects using fluent API. 
There are three types of validation rules which can be created.

1. ValidationRule<TEntity> : Simple validation rule with precondition, validation expression, error message with format parameter.
2. ChildValidationRule<TEntity, TChild> : Validation rule to validate child property.
3. CollectionChildValidationRule<TEntity, TChild> : Validation rule to validate IEnumerabl<TChild> child property.
4. CompositeValidationRule<TEntity> : Validation rule to validate multiple validation rules on certain pre-condition with error message describing the pre-condition..
