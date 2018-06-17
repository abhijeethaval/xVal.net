using NSubstitute;
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
            Assert.IsAssignableFrom<IValidationRule<Employee>>(rule);
        }

        [Fact]
        public void ConstructorThrowsIfMessageFormatIsNull()
        {
            var addressRule = ValidationRule.For<Address>()
                .Validate(a => true)
                .Message("Error message")
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message(null);
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRuleBuilder.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: format", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildExpressionIsNull()
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
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childExprn", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildValidationRuleIsNull()
        {
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(null)
                .Message("Error message");
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRuleBuilder.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childValidationRule", exception.Message);
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
                .Message("City = {0}", a => a.City)
                .Build();
            var employeeRuleBuilder = ValidationRule.For<Employee>()
                .ForChild(e => e.Address)
                .Validate(addressRule)
                .Message("Employee Id = {0}", e => e.Id);
            var result = employeeRuleBuilder.Build().Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}"
                + Environment.NewLine
                + $"City = {employee.Address.City}");
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        private Employee GetEmployee()
        {
            return new Employee
            {
                Id = 1,
                Firstname = "Sandeep",
                Lastname = "Morwal",
                Address = new Address
                {
                    City = "Mumbai",
                    Street = "Marine Drive",
                }
            };
        }
    }
}
