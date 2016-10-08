using XVal.Core.Tests.TestData;

namespace XVal.Core.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstnameRule = ValidationRule.For<Employee>()
                 .Validate(e => e.Firstname != null)
                 .When(e => e.Id != null)
                 .Message("Firstname is mandatory. Id = {0}", e => e.Id)
                 .Build();

            var lastnameRule = ValidationRule.For<Employee>()
                .Validate(e => e.Lastname != null)
                 .When(e => e.Id != null)
                .Message("Lastname is mandatory. I.When(e => e.Id != null)d = {0}", e => e.Id)
                .Build();

            var compositeRule = ValidationRule.For<Employee>()
                .Validate(firstnameRule, lastnameRule)
                .When(e => e.Id != null)
                .Message("Validations failed. Id = {0}, Firstname = {1}", e => e.Id, e => e.Firstname);

            var addressRule = ValidationRule.For<Address>()
                .Validate(a => a.City != null)
                .Message("City is required.")
                .Build();

            var propertyRule = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .When(e => e.Id != null)
                .Message("Employee Id = {0}", e => e.Id)
                .Build();

            var contactNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => p.Number != null)
                .Message("Phone number is required.")
                .Build();

            var enumerableChildrenRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(contactNumberRule)
                .When(e => e.Id != null)
                .Message("")
                .Build();
        }
    }
}
