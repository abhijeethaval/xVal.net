using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class CollectionChildValidationRuleTests
    {
        [Fact]
        public void ImplementsIValidationRule()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule)
                .Message("Error message")
                .Build();

            Assert.IsAssignableFrom<IValidationRule<Employee>>(employeeRule);
        }

        [Fact]
        public void ConstructorThrowsIfMessageFormatIsNull()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule);
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRule.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: format", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildExpressionIsNull()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren<PhoneNumber>(null)
                .Validate(phoneNumberRule)
                .Message("Error message");
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRule.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: collection", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildValidationRuleIsNull()
        {
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(null)
                .Message("Error message");
            var exception = Assert.Throws<ArgumentNullException>(() => employeeRule.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childValidationRule", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionIsFalse()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => false)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .When(e => false)
                .Validate(phoneNumberRule)
                .Message("Error message");
            var result = employeeRule.Build().Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildIsNull()
        {
            var employee = GetEmployee();
            employee.ContactNumbers = null;
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => false)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule)
                .Message("Error message");
            var result = employeeRule.Build().Execute(employee);
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenChildRulePassesForAllTheChilds()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule)
                .Message("Error message");

            var result = employeeRule.Build().Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailWhenChildRuleFails()
        {
            var employee = GetEmployee();
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => false)
                .Message("Phone number = {0}", p => p.Number)
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule)
                .Message("Employee Id = {0}", e => e.Id);

            var result = employeeRule.Build().Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}"
                + Environment.NewLine
                + $"Phone number = {employee.ContactNumbers.First().Number}"
                + Environment.NewLine
                + $"Phone number = {employee.ContactNumbers.Skip(1).First().Number}");
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
                },

                ContactNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Number = 123456789, Type = PhoneType.Mobile },
                    new PhoneNumber { Number = 023433344, Type = PhoneType.Home },
                }
            };
        }
    }
}
