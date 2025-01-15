﻿using Dapper;
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
using System.Windows;
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
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateEntry(), canExecute => { return true; });
        #endregion

        #region Constructor
        public PositionPageVM()
        {
            LoadData();
        }
        #endregion

        #region Methods
        private void LoadData()
        {
            using (IDbConnection con = new SQLiteConnection(_connectionString))
            {
                string sqlQuery = "SELECT position_id AS PositionID, position_description AS PositionDescription FROM positions";
                List<Position> positions = con.Query<Position>(sqlQuery).ToList();
                Positions = new ObservableCollection<Position>(positions);
            }
        }
        private void UpdateEntry()
        {
            if (SelectedPosition != null)
            {
                //SelectedPosition.PositionDescription = TbText;
                using (IDbConnection con = new SQLiteConnection(_connectionString))
                {
                    try
                    {
                        string sqlQuery = "UPDATE positions SET position_description = @Description WHERE position_ID = @ID";
                        con.Execute(sqlQuery, new {Description = TbText, ID = SelectedPosition.PositionID});
                        SelectedPosition.PositionDescription = TbText;
                    }
                    catch (SQLiteException ex)
                    {
                        // Handle specific SQLite database errors
                        switch (ex.ErrorCode)
                        {
                            case (int)SQLiteErrorCode.Constraint:
                                MessageBox.Show("A database constraint was violated. Ensure all required fields are filled.",
                                                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            case (int)SQLiteErrorCode.Locked:
                                MessageBox.Show("The database is currently locked. Please try again later.",
                                                "Database Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                break;
                            default:
                                MessageBox.Show($"An unexpected database error occurred: {ex.Message}",
                                                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle other types of general errors
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                                        "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}
