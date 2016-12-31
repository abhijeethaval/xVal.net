# What is it?
xVal.net is a fluent validation library for .NET. It is designed to be flexible.
It allows specifying the validation rules for the .NET objects using fluent API. 
There are three types of validation rules which can be created.

1. ValidationRule<TEntity> : Simple validation rule with precondition, validation expression, error message with format parameter.
2. ChildValidationRule<TEntity, TChild> : Validation rule to validate child property.
3. CollectionChildValidationRule<TEntity, TChild> : Validation rule to validate IEnumerabl<TChild> child property.
4. CompositeValidationRule<TEntity> : Validation rule to validate multiple validation rules on certain pre-condition with error message describing the pre-condition.

#How to use
Following example shows usage of the framework

* Entities for which validation rules are built

```
    public class Employee
    {
        public int? Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public Address Address { get; set; }

        public IEnumerable<PhoneNumber> ContactNumbers { get; set; }
    }

    public class PhoneNumber
    {
        public int? Number { get; set; }

        public PhoneType? Type { get; set; }
    }

    public enum PhoneType
    {
        Mobile,
        Home,
        Office,
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }
```
* Example of simple ValidationRule<TEntity>
```
            var firstnameRule = ValidationRule.For<Employee>()
                 .Validate(e => e.Firstname != null)
                 .When(e => e.Id != null)
                 .Message("Firstname is mandatory. Employee Id = {0}", e => e.Id)
                 .Build();
```
