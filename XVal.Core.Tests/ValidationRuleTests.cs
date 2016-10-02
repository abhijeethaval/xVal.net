using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ValidationRuleTests
    {
        [Fact]
        public void ConstructorThrowsIfMessageFormatterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ValidationRule<Employee>(null, null, e => false));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: messageFormatter", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfValidateExprnIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Error message"), null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: validateExprn", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionIsNull()
        {
            var rule = new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Error message"), e => false);
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionReturnsFalse()
        {
            var rule = new ValidationRule<Employee>(e => false, new MessageFormatter<Employee>("Error message"), e => false);
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenValidateExpressionReturnsTrue()
        {
            var rule = new ValidationRule<Employee>(e => true, new MessageFormatter<Employee>("Error message"), e => true);
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailedWhenValidateExpressonReturnsFalse()
        {
            var employee = new Employee { Id = 1 };
            var rule = new ValidationRule<Employee>(e => true, new MessageFormatter<Employee>("Employee Id = {0}", e => e.Id), e => false);
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(string.Format(string.Format("Employee Id = {0}", employee.Id)));
            Assert.Equal(expected, result);
        }
    }
}
