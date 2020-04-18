using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#pragma warning disable 1587
/// <summary>
/// packages\xunit.runner.console.2.1.0\tools\xunit.console.exe XVal.Core.Tests\bin\Debug\XVal.Core.Tests.dll
/// </summary>
#pragma warning restore 1587
namespace XVal.Core.Tests
{
    public class ValidationResultTests
    {
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

        public static IEnumerable<object[]> CombineResultsData => new[]
        {
            new object[] { ValidationResult.Passed(), ValidationResult.Passed(), ValidationResult.Passed()},
            new object[] { ValidationResult.Passed(), ValidationResult.Failed("Message"), ValidationResult.Failed("Message")},
            new object[] { ValidationResult.Failed("Message"), ValidationResult.Passed(), ValidationResult.Failed("Message")},
            new object[] { ValidationResult.Failed("Message1"), ValidationResult.Failed("Message2"), ValidationResult.Failed("Message1" + Environment.NewLine + "Message2")},
        };

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqualsWorksCorrectly(ValidationResult result, object obj, bool expected) => Assert.Equal(expected, result.Equals(obj));

        [Theory]
        [MemberData(nameof(GenericEqualsTestData))]
        public void GenericEqualsWorksCorrectly(ValidationResult result, ValidationResult other, bool expected) => Assert.Equal(expected, result.Equals(other));

        [Theory]
        [MemberData(nameof(GenericEqualsTestData))]
        public void EqualsOperatorWorksCorrectly(ValidationResult result, ValidationResult other, bool expected) => Assert.Equal(expected, result == other);

        [Theory]
        [MemberData(nameof(GenericEqualsTestData))]
        public void NotEqualsOperatorWorksCorrectly(ValidationResult result, ValidationResult other, bool oppositOfExpcted) => Assert.Equal(!oppositOfExpcted, result != other);

        [Theory]
        [MemberData(nameof(GetHashCodeData))]
        public void GetHashCodeWorksCorrectly(ValidationResult result1, ValidationResult result2, bool shouldHashCodeEqual) => Assert.Equal(shouldHashCodeEqual, result1.GetHashCode() == result2.GetHashCode());

        public static IEnumerable<object[]> EqualsTestData => GenericEqualsTestData.Concat(
            new[]
            {
                new object[]{ValidationResult.Failed("Message1"), new object(), false},
                new object[]{ValidationResult.Failed("Message1"), null, false},
            });

        public static IEnumerable<object[]> GetHashCodeData => GenericEqualsTestData.Concat(
            new[] 
            {
                new object[]{ ValidationResult.Failed("Abcd"), ValidationResult.Failed("Abcd"), true },
                new object[]{ ValidationResult.Failed("abcd efgh"), ValidationResult.Failed("abcd efgh"), true },
                new object[]{ ValidationResult.Failed("some message"), ValidationResult.Failed("some message"), true },

                new object[]{ ValidationResult.Failed("Abcd"), ValidationResult.Failed("Xyz"), false },
                new object[]{ ValidationResult.Failed("abcd efgh"), ValidationResult.Failed("pqrs"), false },
                new object[]{ ValidationResult.Failed("some message"), ValidationResult.Failed("some other message"), false },
            });

        public static IEnumerable<object[]> GenericEqualsTestData
        {
            get
            {
                yield return new object[] { ValidationResult.Passed(), ValidationResult.Passed(), true };
                yield return new object[] { ValidationResult.Failed("Message"), ValidationResult.Failed("Message"), true };
                yield return new object[] { ValidationResult.Passed(), ValidationResult.Failed("Message"), false };
                yield return new object[] { ValidationResult.Failed("Message"), ValidationResult.Passed(), false };
                yield return new object[] { ValidationResult.Failed("Message1"), ValidationResult.Failed("Message2"), false };
            }
        }
    }
}
