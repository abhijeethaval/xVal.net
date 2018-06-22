using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ValidationRuleTests
    {
        [Fact]
        public void MessageMethodThrowsIfMessageFormatIsNull()
        {
            var ruleBuilder = ValidationRule.For<Employee>()
                .Validate(e => true)
                .When(e => true);
            var exception = Assert.Throws<ArgumentNullException>(() => ruleBuilder.Message(null, null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: format", exception.Message);
        }

        [Fact]
        public void BuildThrowsIfMessageFormatIsNotSet()
        {
            var ruleBuilder = ValidationRule.For<Employee>()
                .Validate(e => true)
                .When(e => true);
            var exception = Assert.Throws<ArgumentNullException>(() => ruleBuilder.Build());
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: messageFormatter", exception.Message);
        }

        [Fact]
        public void ValidateThrowsIfValidationExpressionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ValidationRule.For<Employee>()
                    .Validate((Predicate<Employee>)null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: validateExpn", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionReturnsFalse()
        {
            var rule = ValidationRule.For<Employee>()
                .Validate(e => false)
                .When(e => false)
                .Message("Error message")
                .Build();
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenValidateExpressionReturnsTrueAndPreconditionIsNull()
        {
            var rule = ValidationRule.For<Employee>()
                .Validate(e => true)
                .Message("Error message")
                .Build();
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenValidateExpressionReturnsTrueAndPreconditionIsTrue()
        {
            var rule = ValidationRule.For<Employee>()
                .Validate(e => true)
                .When(e => true)
                .Message("Error message")
                .Build();
            var result = rule.Execute(new Employee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailedWhenValidateExpressonReturnsFalseAndPreconditionIsNull()
        {
            var employee = new Employee { Id = 1 };
            var rule = ValidationRule.For<Employee>()
                .Validate(e => false)
                .Message(e => $"Employee Id = {e.Id}")
                .Build();
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(string.Format("Employee Id = {0}", employee.Id));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteReturnFailedWhenValidateExpressonReturnsFalseAndPreconditionIsTrue()
        {
            var employee = new Employee { Id = 1 };
            var rule = ValidationRule.For<Employee>()
                .Validate(e => false)
                .When(e => true)
                .Message(e => $"Employee Id = {e.Id}")
                .Build();
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(string.Format("Employee Id = {0}", employee.Id));
            Assert.Equal(expected, result);
        }
    }
}
