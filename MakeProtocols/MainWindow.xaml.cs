using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private WordMaker WordMaker;

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

            AutomatsList.CollectionChanged += AutomatsList_CollectionChanged;

            dgAutomats.ItemsSource = AutomatsList;
            dgRelayList.ItemsSource = RelayList;
            dgSFs.ItemsSource = SFList;

        }

        private void AutomatsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //
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

            int numRows = rows.Length;
            if (rows.Length > 1) numRows--; // Exclude the last empty row

            if (numRows > 0)
            {
                string[] values = rows[0].Split('\t');

                if (values.Length == 20)
                {
                    // Clear the existing rows if needed
                    AutomatsList.Clear();

                    // Parse and populate the DataGrid
                    for (int i = 0; i < numRows; i++)
                    {

                        values = rows[i].Split('\t'); // Assuming tab-separated values, adjust for other delimiters
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
                                values[17],
                                values[18],
                                values[19]
                            ));
                    }
                }
                else if (values.Length == 1)
                {
                    //DataGridCell gridCell = TryToFindGridCell(dgAutomats, dgAutomats.SelectedCells[1]);
                    //dgAutomats.SelectedValue = values[0].ToString();

                    //DataGridCell dataGridCell = new DataGridCell();
                    //dataGridCell.Content = values[0];
                    //dgAutomats.SelectedItem = dataGridCell;
                }

            }
        }

        //static DataGridCell TryToFindGridCell(DataGrid grid, DataGridCellInfo cellInfo)

        //{

        //    DataGridCell result = null;

        //    DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(cellInfo.Item);

        //    if (row != null)

        //    {

        //        int columnIndex = grid.Columns.IndexOf(cellInfo.Column);

        //        if (columnIndex > -1)

        //        {

        //            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

        //            result = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;

        //        }

        //    }

        //    return result;

        //}



        //static T GetVisualChild<T>(Visual parent) where T : Visual

        //{

        //    T child = default(T);

        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

        //    for (int i = 0; i < numVisuals; i++)

        //    {

        //        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);

        //        child = v as T;

        //        if (child == null)

        //        {

        //            child = GetVisualChild<T>(v);

        //        }

        //        if (child != null)

        //        {

        //            break;

        //        }

        //    }

        //    return child;

        //}

        private void BtnClearAutomats_Click(object sender, RoutedEventArgs e)
        {
            AutomatsList.Clear();
        }

        private void TxtRelayCount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {


            //После заполнения в таблице нужно записать изменения

        }

        private void BtnCreateRelayList_Click(object sender, RoutedEventArgs e)
        {
            RelayGrid.Children.Clear();
            RelayGrid.ColumnDefinitions.Clear();
            RelayGrid.RowDefinitions.Clear();
            RelayGrid.Resources.Clear();
            foreach (Automat automat in AutomatsList)
                if(automat.Relays != null && automat.Relays.Count>0) automat.Relays.Clear();
            RelayList.Clear();

            if (!int.TryParse(txtRelayCount.Text, out int countRelay)) countRelay = 0;

            int countKontaktor = 0;
            if (chKontantorEnable.IsChecked == true) countKontaktor = 1;

            //Заполняем список реле для отображения в таблице
            if (countRelay > 0 || countKontaktor > 0)
            {
                
                RelayGrid.ShowGridLines = true;
                RelayGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });

                //Создание списка реле в Наблюдаемой коллекции
                for (int i = 0; i < countRelay; i++)
                {
                    RelayList.Add(new Relay() { IDrelay = "К" + (i + 1).ToString(), TypeRelay = "К", NameRelay = "", Mark = "Finder\n40.52.8.230.0000\n230 В AC" });
                    RelayGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
                for (int i = 0; i < countKontaktor; i++)
                {
                    RelayList.Add(new Relay() { IDrelay = "КМ" + (i + 1).ToString(), TypeRelay = "КМ", NameRelay = "", Mark = $"" });
                    RelayGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }

                for (int i = 0; i < AutomatsList.Count; i++)
                {
                    AutomatsList[i].Relays = new ObservableCollection<Relay>();

                    //создать строку Грида с наименованием автомата
                    RelayGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    TextBlock textBlock = new TextBlock
                    {
                        Text = AutomatsList[i].NameAutomat,
                        Margin = new Thickness(5)
                    };
                    Grid.SetColumn(textBlock, 0);
                    Grid.SetRow(textBlock, i);
                    RelayGrid.Children.Add(textBlock);

                    //создать колонки Грида с чекбоксами реле для 
                    for (int j = 0; j < RelayList.Count; j++)
                    {
                        if (RelayList[j].TypeRelay == "К") RelayList[j].NameRelay = AutomatsList[i].PositionNumb + RelayList[j].IDrelay;
                        else RelayList[j].NameRelay = AutomatsList[i].Section + "КМ" + AutomatsList[i].PositionNumb;

                        CheckBox checkBox = new CheckBox
                        {
                            Content = RelayList[j].NameRelay,
                            IsChecked = true,
                            Margin = new Thickness(7),
                        };
                        Grid.SetColumn(checkBox, j + 1);
                        Grid.SetRow(checkBox, i);
                        RelayGrid.Children.Add(checkBox);
                        RelayList[j].IsChecked = false;

                        AutomatsList[i].Relays.Add(RelayList[j].Clone());
                    }
                }
            }
        }

        private void BtnSaveRelayList_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;

            for (int i = 0; i < AutomatsList.Count; i++)
            {
                for (int j = 0; j <= RelayList.Count; j++)
                {
                    if(j<RelayList.Count) AutomatsList[i].Relays.Add(RelayList[j].Clone()); //Сохранение настроенной информации в таблице в экземпляр автомата

                    //Чекбокс чтобы знать используется ли реле для этого автомата
                    if (RelayGrid.Children[index].GetType() == typeof(CheckBox))
                    {
                        AutomatsList[i].Relays[j - 1].IsChecked = (RelayGrid.Children[index] as CheckBox).IsChecked;
                    }
                    index++;
                }
            }
        }

        private void BtnCreateSFsList_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSFsCount.Text, out int countSFs)) countSFs = 0;
            for (int i = 0; i <countSFs; i++)
            {
                SFList.Add(new SF() {
                    ID = $"SF{i+1}",
                    Name = "",
                    Type = "ETIMAT 6",
                    Character = "B",
                    Inom = "",
                    CountPhases = "",
                    Ioverload = "",
                    Toverload = "",
                    Ito = "",
                    Tto = ""
                });
            }
        }

        private void BtnSaveSFsList_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i< AutomatsList.Count;i++)
            {
                AutomatsList[i].SFs = new List<SF>();
                for (int j = 0; j<SFList.Count;j++)
                {
                    SFList[j].Name = AutomatsList[i].PositionNumb + SFList[j].ID;
                    AutomatsList[i].SFs.Add(SFList[j].Clone());
                }
            }
        }

        private void BtnMakeWord_Click(object sender, RoutedEventArgs e)
        {
            WordMaker = new WordMaker(AutomatsList);
            ProtocolDocument protocolDocument = new ProtocolDocument();
            if (
                !string.IsNullOrEmpty(txtModules.Text) ||
                !string.IsNullOrEmpty(txtShifr.Text) ||
                !string.IsNullOrEmpty(txtNumbProt.Text) ||
                !string.IsNullOrEmpty(txtShifrUstavok.Text) ||
                !string.IsNullOrEmpty(txtObject.Text) ||
                !string.IsNullOrEmpty(txtDateProtocol.Text)
                )
            {
                protocolDocument.Modules = txtModules.Text;
                protocolDocument.Shifr = txtShifr.Text;
                protocolDocument.NumbProt = txtNumbProt.Text;
                protocolDocument.KartUstav = txtShifrUstavok.Text;
                protocolDocument.ObjectProt = txtObject.Text;
                protocolDocument.DateProt = txtDateProtocol.Text;
            }
            else MessageBox.Show("Нужно заполнить все поля из раздела \"Общие сведения\"", "Обратите внимание");
        }
    }
}
