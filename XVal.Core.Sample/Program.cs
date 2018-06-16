using XVal.Core.Tests.TestData;

namespace XVal.Core.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var employeeRuleBuilder = ValidationRule.For<Employee>();

            var firstnameRule = employeeRuleBuilder
                 .Validate(e => e.Firstname != null)
                 .When(e => e.Id != null)
                 .Message("Firstname is mandatory. Id = {0}", e => e.Id)
                 .Build();

            var lastnameRule = employeeRuleBuilder
                .Validate(e => e.Lastname != null)
                 .When(e => e.Id != null)
                .Message("Lastname is mandatory. I.When(e => e.Id != null)d = {0}", e => e.Id)
                .Build();

            var compositeRule = employeeRuleBuilder
                .Validate(firstnameRule, lastnameRule)
                .When(e => e.Id != null)
                .Message("Validations failed. Id = {0}, Firstname = {1}", e => e.Id, e => e.Firstname);

            var addressRule = ValidationRule.For<Address>()
                .Validate(a => a.City != null)
                .Message("City is required.")
                .Build();

            var propertyRule = employeeRuleBuilder
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .When(e => e.Id != null)
                .Message("Employee Id = {0}", e => e.Id)
                .Build();

            var contactNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => p.Number != null)
                .Message("Phone number is required.")
                .Build();

            var enumerableChildrenRule = employeeRuleBuilder
                .ForChildren(e => e.ContactNumbers)
                .Validate(contactNumberRule)
                .When(e => e.Id != null)
                .Message("some message")
                .Build();
        }
    }
}
