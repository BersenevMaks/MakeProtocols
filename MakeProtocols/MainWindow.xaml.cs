using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

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
            string connectionString = "Data Source = " + Environment.CurrentDirectory + "\\protocolsBase.db; Version = 3;";
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
                if (AutomatsList.Count < 1)
                {
                    string clipboardText = Clipboard.GetText();
                    PasteDataIntoDataGrid(clipboardText);
                    e.Handled = true;
                }
            }
        }

        private void PasteDataIntoDataGrid(string clipboardText)
        {
            string[] rows = clipboardText.Split('\n');
            int numRows = rows.Length - 1; // Exclude the last empty row

            if (numRows > 0)
            {
                // Clear the existing rows if needed
                AutomatsList.Clear();

                // Parse and populate the DataGrid
                for (int i = 0; i < numRows; i++)
                {

                    string[] values = rows[i].Split('\t'); // Assuming tab-separated values, adjust for other delimiters
                    Automat au = new Automat();
                    AutomatsList.Add(au.Factory
                        (
                        values[0],
                        values[1],
                        values[2],
                        values[4],
                        values[6],
                        values[7],
                        values[8],
                        values[9],
                        values[10],
                        values[11],
                        values[12],
                        values[13],
                        values[14],
                        values[15],
                        values[16],
                        values[17]
                        
                        ));
                }
            }
        }

        private void BtnClearAutomats_Click(object sender, RoutedEventArgs e)
        {
            AutomatsList.Clear();
        }
    }
}
