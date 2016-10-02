using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// packages\xunit.runner.console.2.1.0\tools\xunit.console.exe XVal.Core.Tests\bin\Debug\XVal.Core.Tests.dll
/// </summary>
namespace XVal.Core.Tests
{
    public class ValidationResultTests
    {
        [Fact]
        public void FailedThrowsIfMessageIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ValidationResult.Failed(null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: message", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void FailedThrowsIfMessageIsEmptyOrSpace(string message)
        {
            var exception = Assert.Throws<ArgumentException>(() => ValidationResult.Failed(message));
            Assert.Equal("Value cannot be empty string or white space." + Environment.NewLine + "Parameter name: message", exception.Message);
        }

        [Fact]
        public void PassedReturnsPassedResult()
        {
            var passed = ValidationResult.Passed();
            Assert.True(passed.Result);
            Assert.True(passed);
            Assert.Null(passed.Message);
        }

        [Fact]
        public void FailedReturnsFailedResult()
        {
            var message = "Some message";
            var failed = ValidationResult.Failed(message);
            Assert.False(failed.Result);
            Assert.False(failed);
            Assert.Equal(message, failed.Message);
        }

        [Theory]
        [MemberData(nameof(CombineResultsData))]
        public void CombinesResultsCorrectly(ValidationResult result1, ValidationResult result2, ValidationResult expected)
        {
            var combined = ValidationResult.Combine(result1, result2);
            Assert.Equal(expected.Result, combined.Result);
            Assert.Equal(expected.Message, combined.Message);
        }

        public static IEnumerable<object[]> CombineResultsData
        {
            get
            {
                return new[]
                {
                    new object[] { ValidationResult.Passed(), ValidationResult.Passed(), ValidationResult.Passed()},
                    new object[] { ValidationResult.Passed(), ValidationResult.Failed("Message"), ValidationResult.Failed("Message")},
                    new object[] { ValidationResult.Failed("Message"), ValidationResult.Passed(), ValidationResult.Failed("Message")},
                    new object[] { ValidationResult.Failed("Message1"), ValidationResult.Failed("Message2"), ValidationResult.Failed("Message1" + Environment.NewLine + "Message2")},
                };
            }
        }
    }
}
