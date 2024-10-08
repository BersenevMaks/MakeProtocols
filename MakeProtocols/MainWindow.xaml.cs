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
using System.Data.SQLite;

namespace MakeProtocols
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        SQLiteConnection conn;


        public ObservableCollection<Automat> AutomatsList { get; set; }
        public ObservableCollection<Relay> RelayList { get; set; }
        public ObservableCollection<SF> SFList { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetSource();
            string connectionString = "Data Source = "+ Environment.CurrentDirectory + "\\protocolsBase.db; Version = 3;";
            conn = new SQLiteConnection(connectionString);
        }

        public void SetSource()
        {
            AutomatsList = new ObservableCollection<Automat>();
            RelayList = new ObservableCollection<Relay>();
            SFList = new ObservableCollection<SF>();

            dgAutomats.ItemsSource = AutomatsList;
            dgRelayList.ItemsSource = RelayList;
            dgSFs.ItemsSource = SFList;
            
        }


    }
}
