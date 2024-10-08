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

        public ObservableCollection<Automat> AutomatsList;

        public ObservableCollection<Relay> RelayList;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetSource();
        }

        public void SetSource()
        {
            AutomatsList = new ObservableCollection<Automat>();
            RelayList = new ObservableCollection<Relay>();

            for (int i = 0; i < 10; i++)
            {
                AutomatsList.Add(new Automat() { NameAutomat = "Автомат " + i, Description = " ", NominalCurrent = " ", NominalVoltage = " ", NumbVendor = " ", Type = " ", Ust_Ig = " ", Ust_Ii = " ", Ust_Ir=" ", Ust_Isd=" ", Ust_Tg=" ", Ust_Ti = " ", Ust_Tr = " ", Ust_Tsd = " "});
                RelayList.Add(new Relay() { IDrelay = "1", TypeRelay = " " });
            }

            //dgRelayList.ItemsSource = RelayList;
        }


    }
}
