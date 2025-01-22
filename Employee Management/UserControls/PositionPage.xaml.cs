using Employee_Management.Model;
using Employee_Management.Validations;
using Employee_Management.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Employee_Management.UserControls
{
    /// <summary>
    /// Interaction logic for PositionPage.xaml
    /// </summary>
    public partial class PositionPage : UserControl
    {
        public PositionPage()
        {
            InitializeComponent();
            PositionPageVM vm = new PositionPageVM();
            DataContext = vm;

            //// Manually set the validation rules
            //ObservableCollection<IValidationRule> validationRules = new ObservableCollection<IValidationRule>
            //{
            //    new MinCharRule(5),
            //    new SpecialCharRule('@')
            //};

            //// Apply the validation rules to the TextBox
            //ValidationRuleUtil.SetValidationRules(tbDescription, validationRules);
        }
    }
}
