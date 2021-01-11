using System;
using System.Collections.Generic;
using System.Text;

namespace Neighbor.Mobile.Validation
{
    public class CompareEntryRule<T> : IValidationRule<T>
    {
        public object ValidateObject { get; set; }

        public string ComparePropertyName { get; set; }

        public string ValidationMessage { get; set; }

        public CompareEntryRule(object validateObject, string comparePropertyName)
        {
            this.ValidateObject = validateObject;
            ComparePropertyName = comparePropertyName;
        }

        public bool Check(T value)
        {
            var property = ValidateObject.GetType().GetProperty(ComparePropertyName);
            var propertyValue = (ValidatableObject<string>)property.GetValue(ValidateObject);

            var isMatch = Equals(value, propertyValue?.Value);

            return isMatch;
        }
    }
}
