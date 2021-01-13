namespace Neighbor.Mobile.Validation
{
    public class MinLenghtEntryRule<T> : IValidationRule<T>
    {
        private readonly int minLength;

        public string ValidationMessage { get; set; }

        public MinLenghtEntryRule(int minLength)
        {
            this.minLength = minLength;
        }

        public bool Check(T value)
        {
            var stringValue = value?.ToString() ?? string.Empty;
            var isValid = stringValue.Length > minLength;

            return isValid;
        }
    }
}
