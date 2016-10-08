using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class CollectionChildValidationRuleBuilderTests
    {
        [Theory]
        [MemberData(nameof(GetValidationRuleForEmployeeData))]
        public void BuildsValidationRule(CollectionChildValidationRule<Employee, PhoneNumber> expectedRule)
        {
            var rule = new CollectionChildValidationRuleBuilder<Employee, PhoneNumber>(expectedRule.Collection)
                .Validate(expectedRule.ChildValidationRule)
                .When(expectedRule.Precondition)
                .Message(expectedRule.MessageFormatter.Format, expectedRule.MessageFormatter.Arguments.ToArray())
                .Build();
            var employee = GetEmployee();
            Assert.Equal(expectedRule.Precondition(employee), rule.Precondition(employee));
            Assert.Equal(expectedRule.ChildValidationRule, rule.ChildValidationRule);
            Assert.Equal(expectedRule.MessageFormatter.Format, rule.MessageFormatter.Format);
            Assert.Equal(expectedRule.MessageFormatter.Arguments.Count(), rule.MessageFormatter.Arguments.Count());
            foreach (var expectedArgument in expectedRule.MessageFormatter.Arguments)
            {
                Assert.True(rule.MessageFormatter.Arguments.Any(a => a(employee) == expectedArgument(employee)));
            }
        }

        public static IEnumerable<object[]> GetValidationRuleForEmployeeData()
        {
            var childRule = new ValidationRule<PhoneNumber>(null, new MessageFormatter<PhoneNumber>("Some message"), e => true);

            yield return new object[] { new CollectionChildValidationRule<Employee, PhoneNumber>(e => true,
                new MessageFormatter<Employee>("Message Format", e => "Parameter"),
                e => e.ContactNumbers,
                childRule)};

            yield return new object[] { new CollectionChildValidationRule<Employee, PhoneNumber>(e => false,
                new MessageFormatter<Employee>("Message Format", e => "Parameter", e => "Parameter2"),
                e => e.ContactNumbers,
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
