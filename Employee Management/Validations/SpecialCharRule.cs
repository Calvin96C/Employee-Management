using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employee_Management.Validations
{
    public class SpecialCharRule : IValidationRule
    {
        public char SpecialChar { get; set; }
        public SpecialCharRule()
        {
            
        }
        public SpecialCharRule(char specialChar)
        {
            this.SpecialChar = specialChar;
        }
        public ValidationResult Validate(string value, CultureInfo cultureInfo)
        {
            if (!value.Contains(SpecialChar))
            {
                return new ValidationResult(false, $"Your input must contain '{SpecialChar}'.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
