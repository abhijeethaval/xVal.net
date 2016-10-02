namespace XVal.Core
{
    public struct ValidationResult
    {
        public bool Result { get; }

        public string Message { get; }

        public static ValidationResult Passed()
        {
            return new ValidationResult(true, null);
        }

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

        public static implicit operator bool(ValidationResult result)
        {
            return result.Result;
        }
    }
}
