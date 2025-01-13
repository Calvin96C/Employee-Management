using System.Reflection.Metadata;

namespace Employee_Management.Model
{
    /// <summary>
    /// Defines the positions available for employees
    /// </summary>
    public class Position
    {
        public int Position_ID { get; init; }
        public string? Position_description { get; set; }

        //Default constructor for Dapper, no parameters
        public Position(){}

        public Position(string position_description)
        {
            //this.Position_ID = position_ID;
            this.Position_description = position_description;
        }

        
    }
}