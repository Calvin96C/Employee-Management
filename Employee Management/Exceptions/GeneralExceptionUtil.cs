using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Employee_Management.Exceptions
{
    internal static class GeneralExceptionUtil
    {
        internal static void ExceptionHandler(Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                            "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
