using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ChildValidationRuleTests
    {
        [Fact]
        public void ImplementsIValidationRule()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var rule = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message("Error message")
                .Build();
            _ = Assert.IsAssignableFrom<IValidationRule<Employee>>(rule);
        }

        [Fact]
        public void BuilderThrowsIfMessageFormatIsNull()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule);
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRuleBuilder.Message((string)null));
            Assert.Equal("Value cannot be null. (Parameter 'format')", exception.Message);
        }

        [Fact]
        public void BuilderThrowsIfChildExpressionIsNull()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild<Address>(null)
                .Validate(addressRule)
                .Message("Error message");
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRuleBuilder.Build());
            Assert.Equal("Value cannot be null. (Parameter 'childExprn')", exception.Message);
        }

        [Fact]
        public void BuilderThrowsIfChildValidationRuleIsNull()
        {
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(null)
                .Message("Error message");
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRuleBuilder.Build());
            Assert.Equal("Value cannot be null. (Parameter 'childValidationRule')", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionIsFalse()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .When(e => false)
                .Validate(addressRule)
                .Message("Error message");
            var result = employeeRuleBuilder.Build().Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildIsNull()
        {
            var employee = GetEmployee();
            employee.Address = null;
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => false)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message("Error message");
            var result = employeeRuleBuilder.Build().Execute(employee);
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildRulePasses()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message("Error message");
            var result = employeeRuleBuilder.Build().Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailWhenChildRuleFails()
        {
            var employee = GetEmployee();
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => false)
                .Message(a => $"City = {a.City}")
                .Build();
            ChildValidationRule<Employee, Address> employeeRule = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message(e => $"Employee Id = {e.Id}, Employee Name = {e.Firstname}");
            var result = employeeRule.Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}, Employee Name = {employee.Firstname}"
                + Environment.NewLine
                + $"City = {employee.Address.City}");
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        private Employee GetEmployee() => new Employee
        {
            Id = 1,
            Firstname = "Sandeep",
            Lastname = "Morwal",
            Address = new Address
            {
                City = "Mumbai",
            }
        };
    }
}
