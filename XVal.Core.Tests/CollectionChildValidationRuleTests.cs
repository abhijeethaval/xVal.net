﻿using System;
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

            _ = Assert.IsAssignableFrom<IValidationRule<Employee>>(employeeRule);
        }

        [Fact]
        public void BuilderThrowsIfMessageFormatIsNull()
        {
            var phoneNumberRule = ValidationRule.For<PhoneNumber>()
                .Validate(p => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule);
            var exception = Assert.Throws<InvalidOperationException>(() => employeeRule.Build());
            Assert.Equal("Cannot build without message. Please provide message or message formatter by calling Message", exception.Message);
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
            Assert.Equal("Value cannot be null. (Parameter 'collection')", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildValidationRuleIsNull()
        {
            var employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(null)
                .Message("Error message");
            var exception = Assert.Throws<InvalidOperationException>(() => employeeRule.Build());
            Assert.Equal("Cannot build without child rule. Please provide child rule by calling Validate", exception.Message);
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
            CollectionChildValidationRule<Employee, PhoneNumber> employeeRule = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers)
                .Validate(phoneNumberRule)
                .Message(e => $"Employee Id = {e.Id}, Employee Name = {e.Firstname}");

            var result = employeeRule.Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}, Employee Name = {employee.Firstname}"
                + Environment.NewLine
                + $"Phone number = {employee.ContactNumbers.First().Number}"
                + Environment.NewLine
                + $"Phone number = {employee.ContactNumbers.Skip(1).First().Number}");
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
            },

            ContactNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Number = 123456789 },
                    new PhoneNumber { Number = 023433344 },
                }
        };
    }
}
