using System;

namespace XVal.Core
{
    public struct ValidationResult : IEquatable<ValidationResult>
    {
        public bool Result { get; }

        public string? Message { get; }

        public static ValidationResult Passed() => new ValidationResult(true, null);

        public static ValidationResult Failed(string message) => new ValidationResult(false, message);

        private ValidationResult(bool result, string? message)
        {
            Result = result;
            Message = message;
        }

        public static ValidationResult Combine(ValidationResult result1, ValidationResult result2)
            => result1 && result2
            ? result1
            : result1 && !result2
            ? result2
            : !result1 && result2
            ? result1
            : Failed(result1.Message + Environment.NewLine + result2.Message);

        public override bool Equals(object? obj)
            => obj is null
            ? false
            : obj.GetType() != typeof(ValidationResult)
            ? false
            : Equals((ValidationResult)obj);

        public bool Equals(ValidationResult other)
            => other.Result == Result && other.Message == Message;

        public static bool operator ==(ValidationResult a, ValidationResult b) => a.Equals(b);

        public static bool operator !=(ValidationResult a, ValidationResult b) => !(a == b);

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = (hash * 23) + Result.GetHashCode();
                hash = (hash * 23) + Message?.GetHashCode() ?? 0;
                return hash;
            }
        }

        public static implicit operator bool(ValidationResult result) => result.Result;
    }
}
