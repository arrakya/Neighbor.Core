using System.Text.RegularExpressions;

namespace Neighbor.Mobile.Validation
{
    public class RegexEntryRule<T> : IValidationRule<T>
    {
        private readonly string regexPattern;
        private readonly bool followPattern;

        public string ValidationMessage { get; set; }

        public RegexEntryRule(string regexPattern, bool followPattern = true)
        {
            this.regexPattern = regexPattern;
            this.followPattern = followPattern;
        }

        public bool Check(T value)
        {
            var stringValue = value?.ToString() ?? string.Empty;

            var regex = new Regex(regexPattern);
            bool isValid;
            if (followPattern)
            {
                isValid = regex.IsMatch(stringValue);
            }
            else
            {
                isValid = !regex.IsMatch(stringValue);
            }

            return isValid;
        }
    }
}
