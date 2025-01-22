using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employee_Management.Validations
{
    /// <summary>
    /// This rule will check for a minimum character length of a given string.
    /// </summary>
    public class MinCharRule : IValidationRule
    {
        public int MinimumCharacters { get; set; }

        public MinCharRule()
        {
            
        }
        public MinCharRule(int minimumCharacters)
        {
            this.MinimumCharacters = minimumCharacters;
        }

        public ValidationResult Validate(string value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value) || value.Length < MinimumCharacters)
            {
                return new ValidationResult(false, $"Input must be at least {MinimumCharacters} characters long.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
