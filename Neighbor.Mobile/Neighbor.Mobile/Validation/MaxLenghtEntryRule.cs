namespace Neighbor.Mobile.Validation
{
    public class MaxLenghtEntryRule<T> : IValidationRule<T>
    {
        private readonly int maxLength;

        public string ValidationMessage { get; set; }

        public MaxLenghtEntryRule(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public bool Check(T value)
        {
            var stringValue = value?.ToString() ?? string.Empty;
            var isValid = stringValue.Length < maxLength;

            return isValid;
        }
    }
}
