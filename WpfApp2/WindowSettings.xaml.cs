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
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;
using WpfApp2.ViewModel;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        private SettingsViewModel SettingsView = new SettingsViewModel(new Settings());

        public WindowSettings()
        {
            InitializeComponent();
            DataContext = SettingsView;
        }

        private void DragDpor_Drop(object sender, DragEventArgs e)
        {
            SettingsView.DragDrop.Execute(e);
        }
    }
}
