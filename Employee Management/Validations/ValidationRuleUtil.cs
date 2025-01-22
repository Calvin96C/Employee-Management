using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Diagnostics;

namespace Employee_Management.Validations
{
    /// <summary>
    /// This serves as a helper class with a dependency property for user controls. 
    /// With an observable collection, rules can dynamically be applied or removed during runtime.
    /// </summary>
    public static class ValidationRuleUtil
    {
        // Define the attached property for storing validation rules including its getter and setter
        public static readonly DependencyProperty ValidationRulesProperty =
            DependencyProperty.RegisterAttached(
                "ValidationRules",
                typeof(ObservableCollection<IValidationRule>),
                typeof(ValidationRuleUtil),
                new PropertyMetadata(new ObservableCollection<IValidationRule>(), OnValidationRulesChanged));

        // Getter
        public static ObservableCollection<IValidationRule> GetValidationRules(DependencyObject obj)
        {
            return (ObservableCollection<IValidationRule>)obj.GetValue(ValidationRulesProperty);
        }

        // Setter
        public static void SetValidationRules(DependencyObject obj, ObservableCollection<IValidationRule> value)
        {
            obj.SetValue(ValidationRulesProperty, value);
        }

        // Triggered when the ValidationRules attached property changes
        private static void OnValidationRulesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                // Unsubscribe from old event handlers
                textBox.TextChanged -= OnTextChanged;

                // If rules are added, subscribe to the TextChanged event
                if (e.NewValue is ObservableCollection<IValidationRule> rules && rules != null)
                {
                    textBox.TextChanged += OnTextChanged;
                }
            }
        }

        // Handle the TextChanged event
        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("Test");

            if (sender is TextBox textBox)
            {
                // Get the attached validation rules
                var rules = GetValidationRules(textBox);

                // Validate the text
                foreach (var rule in rules)
                {
                    var result = rule.Validate(textBox.Text, null);

                    if (!result.IsValid)
                    {
                        // If a rule fails, set the validation error
                        Validation.MarkInvalid(
                            BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty),
                            new ValidationError(new ExceptionValidationRule(), textBox, result.ErrorContent, null));
                        return; // Stop further validation on the first error
                    }
                }

                // If all rules pass, clear any validation errors
                Validation.ClearInvalid(BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty));
            }
        }
    }
}
