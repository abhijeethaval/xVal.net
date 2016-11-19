using NSubstitute;
using Xunit;
using XVal.Core.Tests.TestData;

namespace XVal.Core.Tests
{
    public class ValidationRuleBuilderSelectorTests
    {
        [Fact]
        public void CanGetValidationRuleBuilderSelector()
        {
            var selector = ValidationRule.For<Employee>();
            Assert.IsType<ValidationRuleBuilderSelector<Employee>>(selector);
        }

        [Fact]
        public void ReturnsValidationRuleBuilder()
        {
            var actual = ValidationRule.For<Employee>()
                .Validate(e => e.Id.HasValue);
            Assert.IsType<ValidationRuleBuilder<Employee>>(actual);
        }

        [Fact]
        public void ReturnsCompositeValidationRuleBuilder()
        {
            var actual = ValidationRule.For<Employee>()
                .Validate(Substitute.For<IValidationRule<Employee>>());
            Assert.IsType<CompositeValidationRuleBuilder<Employee>>(actual);
        }

        [Fact]
        public void ReturnsChildValidationRuleBuilder()
        {
            var actual = ValidationRule.For<Employee>()
                .ForChild(e => e.Address);
            Assert.IsType<ChildValidationRuleBuilder<Employee, Address>>(actual);
        }

        [Fact]
        public void ReturnsCollectionChildValidationRuleBuilder()
        {
            var actual = ValidationRule.For<Employee>()
                .ForChildren(e => e.ContactNumbers);
            Assert.IsType<CollectionChildValidationRuleBuilder<Employee, PhoneNumber>>(actual);
        }
    }
}
