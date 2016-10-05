using Xunit;
using XVal.Core;
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
    }
}
