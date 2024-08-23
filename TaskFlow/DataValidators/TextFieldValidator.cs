using System.Globalization;
using System.Windows.Controls;

namespace TaskFlow.DataValidators
{
    public class TextFieldValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, "Please provide this field");
            }

            return ValidationResult.ValidResult;
        }
    }
}
