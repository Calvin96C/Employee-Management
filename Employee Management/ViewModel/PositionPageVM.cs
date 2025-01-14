using Dapper;
using Employee_Management.Model;
using Employee_Management.MVVM;
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
        #endregion

        #region Properties
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
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveEntry(), canExecute => { return true; });
        #endregion

        #region Constructor
        public PositionPageVM()
        {
            using (IDbConnection con = new SQLiteConnection(_connectionString))
            {
                string sqlQuery = "SELECT position_id AS PositionID, position_description AS PositionDescription FROM positions";
                List<Position> positions = con.Query<Position>(sqlQuery).ToList();
                Positions = new ObservableCollection<Position>(positions);
            }

            //SaveCommand = new RelayCommand
        }
        #endregion

        #region Methods
        private void SaveEntry()
        {
            if (SelectedPosition != null)
            {
                SelectedPosition.PositionDescription = TbText;
            }
        }
        #endregion
    }
}
