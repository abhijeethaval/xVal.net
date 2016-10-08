using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ChildValidationRuleBuilderTests
    {
        [Theory]
        [MemberData(nameof(GetValidationRuleForEmployeeData))]
        public void BuildsValidationRule(ChildValidationRule<Employee, Address> expectedRule)
        {
            var rule = new ChildValidationRuleBuilder<Employee, Address>(expectedRule.ChildExprn)
                .Validate(expectedRule.InternalValidationRule)
                .When(expectedRule.Precondition)
                .Message(expectedRule.MessageFormatter.Format, expectedRule.MessageFormatter.Arguments.ToArray())
                .Build();
            var employee = GetEmployee();
            Assert.Equal(expectedRule.Precondition(employee), rule.Precondition(employee));
            Assert.Equal(expectedRule.InternalValidationRule, rule.InternalValidationRule);
            Assert.Equal(expectedRule.MessageFormatter.Format, rule.MessageFormatter.Format);
            Assert.Equal(expectedRule.MessageFormatter.Arguments.Count(), rule.MessageFormatter.Arguments.Count());
            foreach (var expectedArgument in expectedRule.MessageFormatter.Arguments)
            {
                Assert.True(rule.MessageFormatter.Arguments.Any(a => a(employee) == expectedArgument(employee)));
            }
        }

        public static IEnumerable<object[]> GetValidationRuleForEmployeeData()
        {
            var childRule = new ValidationRule<Address>(null, new MessageFormatter<Address>("Some message"), e => true);

            yield return new object[] { new ChildValidationRule<Employee, Address>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter"),
                e => e.Address,
                childRule)};

            yield return new object[] { new ChildValidationRule<Employee, Address>(e => false,
                new MessageFormatter<Employee>("Message Format", e => "Parameter"),
                e => e.Address,
                childRule)};
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
