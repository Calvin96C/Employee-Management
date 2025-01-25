using Dapper;
using Employee_Management.Model;
using Employee_Management.MVVM;
using Employee_Management.Utils;
using Employee_Management.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;

namespace Employee_Management.ViewModel
{
    internal class PositionPageVM : ViewModelBase
    {
        #region Private variables/fields
		private ObservableCollection<Position>? _positions;
        private string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        private Position? _selectedPosition;
        private string _tbText;
        private SQLiteUtil.TableStructure _tableStructure;
        #endregion

        #region Properties
        public ObservableCollection<IValidationRule> ValidationRules { get; set; }
        public string? TbText
                {
                    get { return _tbText; }
                    set
                    {
                        _tbText = value;
                        OnPropertyChanged(nameof(TbText));
                    }
                }
        public ObservableCollection<Position> Positions
		{
			get { return _positions!; }
			set 
            { 
                _positions = value;
                OnPropertyChanged(nameof(Positions));
            }
        }
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
                TbText = value.PositionDescription;
            }
        }
        #endregion

        #region Relay Commands
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateEntry(), canExecute => { return true; });
        #endregion

        #region Constructor
        public PositionPageVM()
        {
            _tableStructure = new SQLiteUtil.TableStructure("positions");
            LoadData();
            ValidationRules = new ObservableCollection<IValidationRule>
            {
                //new MinCharRule(5),
                new EmptyTextRule(true)
            };
        }
        #endregion

        #region Methods
        private void LoadData()
        {
            string sqlQuery = "SELECT position_id AS PositionID, position_description AS PositionDescription FROM positions";
            List<Position> positions = SQLiteUtil.ReadData<Position>(sqlQuery);
            Positions = new ObservableCollection<Position>(positions);
        }
        private void UpdateEntry()
        {
            if (SelectedPosition != null && string.IsNullOrEmpty(TbText) == false)
            {
                string sqlQuery = "UPDATE positions SET position_description = @Value WHERE position_ID = @ID";
                object parameters = new {Value = TbText, ID = SelectedPosition.PositionID };
                if (SQLiteUtil.UpdateEntry(sqlQuery, parameters))
                {
                    MessageBox.Show("Entry updated.");
                    SelectedPosition.PositionDescription = TbText;
                }
                else
                {
                    MessageBox.Show("Update failed.");
                }
            }
        }
        #endregion
    }
}
