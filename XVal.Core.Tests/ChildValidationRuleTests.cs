using NSubstitute;
using System;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ChildValidationRuleTests
    {
        [Fact]
        public void ImplementsIValidationRule()
        {
            var rule = new ChildValidationRule<Employee, Address>(null,
                Substitute.For<MessageFormatter<Employee>>("dummyMessage"),
                Substitute.For<Func<Employee, Address>>(),
                Substitute.For<IValidationRule<Address>>());

            Assert.IsAssignableFrom<IValidationRule<Employee>>(rule);
        }

        [Fact]
        public void ConstructorThrowsIfMessageFormatterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ChildValidationRule<Employee, Address>(null, null, Substitute.For<Func<Employee, Address>>(), Substitute.For<IValidationRule<Address>>()));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: messageFormatter", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildExpressionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ChildValidationRule<Employee, Address>(null, Substitute.For<MessageFormatter<Employee>>("dummyMessage"), null, Substitute.For<IValidationRule<Address>>()));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childExprn", exception.Message);
        }

        [Fact]
        public void ConstructorThrowsIfChildValidationRuleIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ChildValidationRule<Employee, Address>(null, Substitute.For<MessageFormatter<Employee>>("dummyMessage"), Substitute.For<Func<Employee, Address>>(), null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: childValidationRule", exception.Message);
        }

        [Fact]
        public void ExecuteReturnsPassedWhenPreconditionReturnsFalse()
        {
            var rule = new ChildValidationRule<Employee, Address>(null,
                Substitute.For<MessageFormatter<Employee>>("dummyMessage"),
                Substitute.For<Func<Employee, Address>>(),
                Substitute.For<IValidationRule<Address>>());

            var result = rule.Execute(GetEmployee());
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnFailWhenChildRuleFails()
        {
            var employee = GetEmployee();
            var rule = new ChildValidationRule<Employee, Address>(e => true,
                GetEmployeeIdFormatter(),
                e => e.Address,
                GetFailingValidationRule());
            var result = rule.Execute(employee);
            var expected = ValidationResult.Failed(GetEmployeeIdFormatter().GetMessage(employee)
                + Environment.NewLine
                + GetCityFormatter().GetMessage(employee.Address));
            Assert.Equal(expected.Result, result.Result);
            Assert.Equal(expected.Message, result.Message);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildIsNull()
        {
            var employee = GetEmployee();
            employee.Address = null;
            var rule = new ChildValidationRule<Employee, Address>(e => true,
                GetEmployeeIdFormatter(),
                e => e.Address,
                GetFailingValidationRule());
            var result = rule.Execute(employee);
            Assert.True(result);
        }

        [Fact]
        public void ExecuteReturnPassedWhenChildRulePasses()
        {
            var employee = GetEmployee();
            var rule = new ChildValidationRule<Employee, Address>(e => true,
                GetEmployeeIdFormatter(),
                e => e.Address,
                GetPassingValidationRule());
            var result = rule.Execute(employee);
            Assert.True(result);

        }

        private static MessageFormatter<Employee> GetEmployeeIdFormatter()
        {
            return new MessageFormatter<Employee>("Employee Id = {0}", e => e.Id);
        }

        private static MessageFormatter<Address> GetCityFormatter()
        {
            return new MessageFormatter<Address>("City = {0}", a => a.City);
        }

        private static ValidationRule<Address> GetPassingValidationRule()
        {
            return new ValidationRule<Address>(null, GetCityFormatter(), e => true);
        }

        private static ValidationRule<Address> GetFailingValidationRule()
        {
            return new ValidationRule<Address>(null, GetCityFormatter(), e => false);
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
