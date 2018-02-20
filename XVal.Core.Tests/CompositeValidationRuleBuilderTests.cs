using System.Collections.Generic;
using System.Linq;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class CompositeValidationRuleBuilderTests
    {
        [Theory]
        [MemberData(nameof(GetValidationRuleForEmployeeData))]
        public void BuildsValidationRule(CompositeValidationRule<Employee> expectedRule)
        {
            var rule = new CompositeValidationRuleBuilder<Employee>(expectedRule.ChildRules.ToArray())
                .When(expectedRule.Precondition)
                .Message(expectedRule.MessageFormatter.Format, expectedRule.MessageFormatter.Arguments.ToArray())
                .Build();
            var employee = new Employee { Id = 1, Firstname = "Suresh", Lastname = "Kamble" };
            Assert.Equal(expectedRule.Precondition(employee), rule.Precondition(employee));
            Assert.Equal(expectedRule.ChildRules.Count(), rule.ChildRules.Count());
            foreach (var expectedChildRule in expectedRule.ChildRules)
            {
                rule.ChildRules.Any(c => c == expectedChildRule);
            }

            Assert.Equal(expectedRule.MessageFormatter.Format, rule.MessageFormatter.Format);
            Assert.Equal(expectedRule.MessageFormatter.Arguments.Count(), rule.MessageFormatter.Arguments.Count());
            foreach (var expectedArgument in expectedRule.MessageFormatter.Arguments)
            {
                Assert.Contains(rule.MessageFormatter.Arguments, a => a(employee) == expectedArgument(employee));
            }
        }

        public static IEnumerable<object[]> GetValidationRuleForEmployeeData()
        {
            var childRule1 = new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Some message"), e => true);
            var childRule2 = new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Some message"), e => true);
            var childRule3 = new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Some message"), e => true);
            var childRule4 = new ValidationRule<Employee>(null, new MessageFormatter<Employee>("Some message"), e => true);

            yield return new object[] { new CompositeValidationRule<Employee>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter"),
                childRule1.ToEnumerable())};

            yield return new object[] { new CompositeValidationRule<Employee>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter1", e => "Parameter2"),
                new [] {childRule1, childRule2}) };

            yield return new object[] { new CompositeValidationRule<Employee>(e => false,
                new MessageFormatter<Employee>("Message Format", e => "Parameter1", e => "Parameter2", e => "Parameter3"),
                new [] {childRule1, childRule2, childRule3}) };

            yield return new object[] { new CompositeValidationRule<Employee>(e => false,
                new MessageFormatter<Employee>("Message Format"),
                new [] {childRule1, childRule2, childRule3, childRule4}) };
        }
    }
}
