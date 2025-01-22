using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace Employee_Management.Validations
{
    public interface IValidationRule
    {
        /// <summary>
        /// Validates a given value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns>ValidationResult indicating whether validation succeeded or failed.</returns>
        ValidationResult Validate(string value, CultureInfo cultureInfo);
    }
}
