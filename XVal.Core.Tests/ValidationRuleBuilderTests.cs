using System.Collections.Generic;
using System.Linq;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ValidationRuleBuilderTests
    {
        [Theory]
        [MemberData(nameof(GetValidationRuleForEmployeeData))]
        public void BuildsValidationRule(ValidationRule<Employee> expectedRule)
        {
            var rule = new ValidationRuleBuilder<Employee>(expectedRule.ValidateExprn)
                .When(expectedRule.Precondition)
                .Message(expectedRule.MessageFormatter.Format, expectedRule.MessageFormatter.Arguments.ToArray())
                .Build();
            var employee = new Employee { Id = 1, Firstname = "Suresh", Lastname = "Kamble" };
            Assert.Equal(expectedRule.Precondition(employee), rule.Precondition(employee));
            Assert.Equal(expectedRule.ValidateExprn(employee), rule.ValidateExprn(employee));

            Assert.Equal(expectedRule.MessageFormatter.Format, rule.MessageFormatter.Format);
            Assert.Equal(expectedRule.MessageFormatter.Arguments.Count(), rule.MessageFormatter.Arguments.Count());
            foreach (var expectedArgument in expectedRule.MessageFormatter.Arguments)
            {
                Assert.True(rule.MessageFormatter.Arguments.Any(a => a(employee) == expectedArgument(employee)));
            }
        }

        public static IEnumerable<object[]> GetValidationRuleForEmployeeData()
        {
            yield return new object[] { new ValidationRule<Employee>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter"),
                e => true) };

            yield return new object[] { new ValidationRule<Employee>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter1", e => "Parameter2"),
                e => false) };

            yield return new object[] { new ValidationRule<Employee>(e => false,
                new MessageFormatter<Employee>("Message Format", e => "Parameter1", e => "Parameter2", e => "Parameter3"),
                e => true) };

            yield return new object[] { new ValidationRule<Employee>(e => false,
                new MessageFormatter<Employee>("Message Format"),
                e => false) };
        }
    }
}
