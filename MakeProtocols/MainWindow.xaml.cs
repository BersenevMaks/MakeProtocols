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

        private void DgAutomats_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.V && (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) == System.Windows.Input.ModifierKeys.Control)
            {
                string clipboardText = Clipboard.GetText();
                PasteDataIntoDataGrid(clipboardText);
                e.Handled = true;
            }
        }

        private void PasteDataIntoDataGrid(string clipboardText)
        {
            string[] rows = clipboardText.Split('\n');
            int numRows = rows.Length - 1; // Exclude the last empty row

            if (numRows > 0)
            {
                // Clear the existing rows if needed
                dgAutomats.Items.Clear();

                // Parse and populate the DataGrid
                for (int i = 0; i < numRows; i++)
                {
                    string[] values = rows[i].Split('\t'); // Assuming tab-separated values, adjust for other delimiters
                    Automat au = new Automat();
                    dgAutomats.Items.Add(au.Factory(values[0],values[1], values[2], values[4]);
                }
            }
        }
    }
}
