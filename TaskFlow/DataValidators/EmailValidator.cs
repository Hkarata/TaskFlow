using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace TaskFlow.DataValidators;

public partial class EmailValidator : ValidationRule
{
    private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        var email = value as string;

        if (string.IsNullOrEmpty(email))
        {
            return new ValidationResult(false, "Please provide your email address");
        }

        var regex = MyRegex();

        return regex.IsMatch(email) ? new ValidationResult(true, null) : new ValidationResult(false, "Please provide a valid email address");
    }

    [GeneratedRegex(EmailRegex)]
    private static partial Regex MyRegex();
}