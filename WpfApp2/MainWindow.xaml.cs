using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using WpfApp2.PingStatus;
using WpfApp2.Infomation;
using WpfApp2.BaseCommands;
using WpfApp2.ViewModel;
using System.Windows.Markup;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.Apis.Sheets.v4.Data;
using System.Xml.Serialization;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IStatusInternet status = CheckStatus.Ping();
        public MainWindow()
        {
            InitializeComponent();
            //свзяь статуса онлайн/офлайн и дерева информации
            DataContext = status;
            treeView.ItemsSource = status.TreeInfas.InfaList;
        }
    }
}
