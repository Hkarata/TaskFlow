using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace TaskFlow.DataValidators
{
    public partial class PasswordValidator : ValidationRule
    {
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult(false, "Please provide your password");
            }

            var regex = MyRegex();

            return regex.IsMatch(password) ? ValidationResult.ValidResult :
                new ValidationResult(false, "At least 8 characters long, contains both uppercase and lowercase letters, at least one digit and a symbol");
        }

        [GeneratedRegex(PasswordRegex)]
        private static partial Regex MyRegex();
    }
}
