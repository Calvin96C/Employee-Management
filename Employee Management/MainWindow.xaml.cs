using Dapper;
using Employee_Management.Model;
using Employee_Management.UserControls;
using Employee_Management.ViewModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace Employee_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        private readonly PositionPage _positionPage = new PositionPage();

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = _positionPage;
            //showPositions();
        }

        private void showPositions()
        {
            string sql_query = "SELECT position_id AS PositionID, position_description AS PositionDescription FROM positions";

            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                List<Position> positionList = connection.Query<Position>(sql_query).ToList();

                foreach (Position position in positionList)
                {
                    MessageBox.Show($"id : {position.PositionID} \nposition: {position.PositionDescription}");
                }
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button has been pressed.");
        }

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else if(WindowState == WindowState.Maximized)
            {
                WindowState= WindowState.Normal;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}