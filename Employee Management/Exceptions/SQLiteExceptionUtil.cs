using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Employee_Management.Exceptions
{
    internal static class SQLiteExceptionUtil
    {
        internal static void ExceptionHandler(SQLiteException ex)
        {
            // Handle specific SQLite database errors

            switch ((SQLiteErrorCode)ex.ErrorCode)
            {
                case SQLiteErrorCode.Constraint:
                    MessageBox.Show("A database constraint was violated. Ensure all required fields are filled.",
                                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case SQLiteErrorCode.Locked:
                    MessageBox.Show("The database is currently locked. Please try again later.",
                                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                default:
                    MessageBox.Show($"An unexpected database error occurred: {ex.Message}",
                                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
