using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class CompositeValidationRuleTests
    {
        [Fact]
        public void ConstructorThrowsIfMessageFormatterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CompositeValidationRule<Employee>(null, null, GetPassingValidationRule().ToEnumerable()));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: messageFormatter", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildRulesIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CompositeValidationRule<Employee>(null, GetEmployeeIdFormatter(), null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childRules", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionReturnsFalse()
        {
            var rule = new CompositeValidationRule<Employee>(e => false, GetEmployeeIdFormatter(), GetPassingValidationRule().ToEnumerable());
            var result = rule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsFailedWhenOneChildRuleReturnsFalse()
        {
            var employee = GetEmployee();
            var rule = new CompositeValidationRule<Employee>(e => true,
                GetEmployeeNameFormatter(),
                GetFailingValidationRule().ToEnumerable());
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(GetEmployeeNameFormatter().GetMessage(employee)
                + Environment.NewLine
                + GetEmployeeIdFormatter().GetMessage(employee));
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        [Fact]
        public void ExecuteReturnsFailedWhenMoreThanOneChildRuleReturnsFalse()
        {
            var employee = GetEmployee();
            var rule = new CompositeValidationRule<Employee>(null,
                GetEmployeeNameFormatter(),
                new[] { GetFailingValidationRule(), GetFailingValidationRule() });
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(GetEmployeeNameFormatter().GetMessage(employee)
                + Environment.NewLine
                + GetEmployeeIdFormatter().GetMessage(employee)
                + Environment.NewLine
                + GetEmployeeIdFormatter().GetMessage(employee));
            //Assert.Equal(expected, result);
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        private static ValidationRule<Employee> GetPassingValidationRule()
        {
            return new ValidationRule<Employee>(null, GetEmployeeIdFormatter(), e => true);
        }

        private static ValidationRule<Employee> GetFailingValidationRule()
        {
            return new ValidationRule<Employee>(null, GetEmployeeIdFormatter(), e => false);
        }

        private static MessageFormatter<Employee> GetEmployeeIdFormatter()
        {
            return new MessageFormatter<Employee>("Employee Id = {0}", e => e.Id);
        }

        private static MessageFormatter<Employee> GetEmployeeNameFormatter()
        {
            return new MessageFormatter<Employee>("Employee Name = {0} {1}", e => e.Firstname, e => e.Lastname);
        }

        private Employee GetEmployee()
        {
            return new Employee
            {
                Id = 1,
                Firstname = "Sandeep",
                Lastname = "Morwal",
            };
        }
    }
}
