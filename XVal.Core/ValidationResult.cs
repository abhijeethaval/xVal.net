using System;

namespace XVal.Core
{
    public struct ValidationResult : IEquatable<ValidationResult>
    {
        public bool Result { get; }

        public string Message { get; }

        public static ValidationResult Passed() => new ValidationResult(true, null);

        public static ValidationResult Failed(string message)
        {
            message.ThrowIfArgumentNullOrWhiteSpace(nameof(message));
            return new ValidationResult(false, message);
        }

        private ValidationResult(bool result, string message)
        {
            Result = result;
            Message = message;
        }

        public static ValidationResult Combine(ValidationResult result1, ValidationResult result2)
        {
            if (result1 && result2)
            {
                return result1;
            }

            if (result1 && !result2)
            {
                return result2;
            }

            if (!result1 && result2)
            {
                return result1;
            }

            return Failed(result1.Message + System.Environment.NewLine + result2.Message);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(ValidationResult))
            {
                return false;
            }

            return Equals((ValidationResult)obj);
        }

        public bool Equals(ValidationResult other)
        {
            return other.Result == Result && other.Message == Message;
        }

        public static bool operator ==(ValidationResult a, ValidationResult b) => a.Equals(b);

        public static bool operator !=(ValidationResult a, ValidationResult b) => !(a == b);

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Result.GetHashCode();
                if (Message != null)
                {
                    hash = hash * 23 + Message.GetHashCode();
                }

                return hash;
            }
        }

        public static implicit operator bool(ValidationResult result) => result.Result;
    }
}
