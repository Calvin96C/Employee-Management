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
    /// This rule will check for an empty string with the option to check for white space.
    /// </summary>
    public class EmptyTextRule : IValidationRule
    {
        public bool CheckWhiteSpace { get; set; }

        public EmptyTextRule()
        {
        }
        public EmptyTextRule(bool checkWhiteSpace)
        {
            this.CheckWhiteSpace= checkWhiteSpace;
        }

        //public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        //{
        //    if (CheckWhiteSpace)
        //    {
        //        if (string.IsNullOrWhiteSpace(value as string))
        //        {
        //            return new ValidationResult(false, "Input cannot be empty nor empty spaces.");
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(value as string))
        //        {
        //            return new ValidationResult(false, "Input cannot be empty.");
        //        }
        //    }
        //    return ValidationResult.ValidResult;
        //}

        public ValidationResult Validate(string value, CultureInfo cultureInfo)
        {
            if (CheckWhiteSpace)
            {
                if (string.IsNullOrWhiteSpace(value as string))
                {
                    return new ValidationResult(false, "Input cannot be empty nor empty spaces.");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return new ValidationResult(false, "Input cannot be empty.");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
