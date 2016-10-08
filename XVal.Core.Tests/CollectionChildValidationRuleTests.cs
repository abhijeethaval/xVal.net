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
            var rule = new CollectionChildValidationRule<Employee, PhoneNumber>(e => true,
                Substitute.For<MessageFormatter<Employee>>("dummyMessage"),
                Substitute.For<Func<Employee, IEnumerable<PhoneNumber>>>(),
                Substitute.For<IValidationRule<PhoneNumber>>());

            Assert.IsAssignableFrom<IValidationRule<Employee>>(rule);
        }

        [Fact]
        public void ConstructorThrowsIfMessageFormatterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CollectionChildValidationRule<Employee, PhoneNumber>(null, null, e => e.ContactNumbers, Substitute.For<IValidationRule<PhoneNumber>>()));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: messageFormatter", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildExpressionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CollectionChildValidationRule<Employee, PhoneNumber>(null, Substitute.For<MessageFormatter<Employee>>("dummyMessage"), null, Substitute.For<IValidationRule<PhoneNumber>>()));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: collection", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildValidationRuleIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CollectionChildValidationRule<Employee, PhoneNumber>(null, Substitute.For<MessageFormatter<Employee>>("dummyMessage"), e => e.ContactNumbers, null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childValidationRule", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionIsFalse()
        {
            var rule = new CollectionChildValidationRule<Employee, PhoneNumber>(e => false,
                Substitute.For<MessageFormatter<Employee>>("Message"),
                e => e.ContactNumbers,
                GetFailingValidationRule());

            var result = rule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildIsNull()
        {
            var employee = GetEmployee();
            employee.ContactNumbers = null;
            var rule = new CollectionChildValidationRule<Employee, PhoneNumber>(e => true,
                GetEmployeeIdFormatter(),
                e => e.ContactNumbers,
                GetFailingValidationRule());
            var result = rule.Execute(employee);
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenChildRulePassesForAllTheChilds()
        {
            var rule = new CollectionChildValidationRule<Employee, PhoneNumber>(null,
                GetEmployeeIdFormatter(),
                e => e.ContactNumbers,
                GetPassingValidationRule());

            var result = rule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailWhenChildRuleFails()
        {
            var employee = GetEmployee();
            var rule = new CollectionChildValidationRule<Employee, PhoneNumber>(e => true,
                GetEmployeeIdFormatter(),
                e => e.ContactNumbers,
                GetFailingValidationRule());
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(GetEmployeeIdFormatter().GetMessage(employee)
                + Environment.NewLine
                + GetPhoneNumberFormatter().GetMessage(employee.ContactNumbers.First())
                + Environment.NewLine
                + GetPhoneNumberFormatter().GetMessage(employee.ContactNumbers.Skip(1).First()));
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        private static MessageFormatter<Employee> GetEmployeeIdFormatter()
        {
            return new MessageFormatter<Employee>("Employee Id = {0}", e => e.Id);
        }

        private static MessageFormatter<PhoneNumber> GetPhoneNumberFormatter()
        {
            return new MessageFormatter<PhoneNumber>("Phone number = {0}", p => p.Number);
        }

        private static ValidationRule<PhoneNumber> GetPassingValidationRule()
        {
            return new ValidationRule<PhoneNumber>(null, GetPhoneNumberFormatter(), e => true);
        }

        private static ValidationRule<PhoneNumber> GetFailingValidationRule()
        {
            return new ValidationRule<PhoneNumber>(null, GetPhoneNumberFormatter(), e => false);
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
