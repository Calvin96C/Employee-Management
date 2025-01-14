using System.ComponentModel;
using System.Reflection.Metadata;

namespace Employee_Management.Model
{
    /// <summary>
    /// Defines the positions available for employees
    /// </summary>
    public class Position : INotifyPropertyChanged
    {
        private int _positionID;
        private string? _positionDescription;

        public int PositionID 
        { 
            get => _positionID;
            set
            {
                _positionID = value;
                OnPropertyChanged(nameof(PositionID));
            }
        }
        public string? PositionDescription 
        { 
            get => _positionDescription; 
            set
            {
                _positionDescription = value;
                OnPropertyChanged(nameof(PositionDescription));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        //Default constructor for Dapper, no parameters
        public Position(){}

        public Position(int position_ID, string position_description)
        {
            this.PositionID = position_ID;
            this.PositionDescription = position_description;
        }
    }
}