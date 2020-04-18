using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class CompositeValidationRuleTests
    {
        [Fact]
        public void BuilderThrowsIfMessageFormatIsNull()
        {
            var childRule = ValidationRule.For<Employee>()
                .Validate(e => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .Validate(childRule);
            var exception = Assert.Throws<InvalidOperationException>(() => employeeRule.Build());
            Assert.Equal("Cannot build without message. Please provide message or message formatter by calling Message", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenChildRuleIsNull()
        {
            var employeeRule = ValidationRule.For<Employee>()
                .Validate((ValidationRule<Employee>)null)
                .Message("Error message")
                .Build();
            var result = employeeRule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenChildRulesIsEmpty()
        {
            var employeeRule = ValidationRule.For<Employee>()
                .Validate()
                .Message("Error message")
                .Build();
            var result = employeeRule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionIsFalse()
        {
            var childRule = ValidationRule.For<Employee>()
                .Validate(e => false)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .Validate(childRule)
                .When(e => false)
                .Message("Error message")
                .Build();
            var result = employeeRule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenChildRuleReturnsPassed()
        {
            var childRule = ValidationRule.For<Employee>()
                .Validate(e => true)
                .Message("Error message")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .Validate(childRule)
                .Message("Error message")
                .Build();
            var result = employeeRule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsFailedWhenOneChildRuleReturnsFalse()
        {
            var childRule1 = ValidationRule.For<Employee>()
                .Validate(e => false)
                .Message("Employee Name = {0} {1}", e => e.Firstname, e => e.Lastname)
                .Build();
            var childRule2 = ValidationRule.For<Employee>()
                .Validate(e => true)
                .Message(e => $"City = {e.Address.City}")
                .Build();
            var employeeRule = ValidationRule.For<Employee>()
                .Validate(childRule1, childRule2)
                .Message(e => $"Employee Id = {e.Id}, Employee Name = {e.Firstname}")
                .Build();
            var employee = GetEmployee();
            var result = employeeRule.Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}, Employee Name = {employee.Firstname}"
                + Environment.NewLine
                + $"Employee Name = {employee.Firstname} {employee.Lastname}");
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        [Fact]
        public void ExecuteReturnsFailedWhenMoreThanOneChildRuleReturnsFalse()
        {
            var childRule1 = ValidationRule.For<Employee>()
                .Validate(e => false)
                .Message("Employee Name = {0} {1}", e => e.Firstname, e => e.Lastname)
                .Build();
            var childRule2 = ValidationRule.For<Employee>()
                .Validate(e => false)
                .Message("City = {0}", e => e.Address.City)
                .Build();
            CompositeValidationRule<Employee> employeeRule = ValidationRule.For<Employee>()
                .Validate(childRule1, childRule2)
                .Message("Employee Id = {0}", e => e.Id);
            var employee = GetEmployee();
            var result = employeeRule.Execute(employee);
            var expected = ValidationResult.Failed($"Employee Id = {employee.Id}"
                                                   + Environment.NewLine
                                                   + $"Employee Name = {employee.Firstname} {employee.Lastname}"
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
                City = "Pune"
            }
        };
    }
}
