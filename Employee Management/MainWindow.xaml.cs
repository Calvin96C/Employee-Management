using Employee_Management.ViewModel;
using System.Configuration;
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

        //private ObservableCollection<Position> _positions;

        //public ObservableCollection<Position> Positions
        //{
        //    get { return _positions; }
        //    set { _positions = value; }
        //}


        public MainWindow()
        {
            //DataContext = this;
            //_positions = new ObservableCollection<Position>();
            InitializeComponent();
            MainWindowVM vm = new MainWindowVM();
            DataContext = vm;
        }

        //private void showPositions()
        //{
        //    string sql_query = "Select * from positions";

        //    using (IDbConnection connection = new SQLiteConnection(_connectionString))
        //    {
        //        List<Position> positions = connection.Query<Position>(sql_query).ToList();
                
        //        //foreach (Position position in positions)
        //        //{
        //        //    MessageBox.Show($"ID : {position.Position_ID} \nPosition: {position.Position_description}");
        //        //}

                
        //        //MessageBox.Show("Positions loaded");
        //    }
        //}

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button has been pressed.");
        }
    }
}