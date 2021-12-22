using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using WpfApp2.BaseCommands;
using System.Xml.Serialization;
using WpfApp2.ViewModel;
using System.Windows.Controls;

namespace WpfApp2.Infomation
{
    [Serializable]
    public class Infas
    {
        public Infas()
        {
            InfaList = new ObservableCollection<Infa>();
        }

        public ObservableCollection<Infa> InfaList { get; set; }
    }
    [Serializable]
    public class Infa : INotifyPropertyChanged
    {
        public Infa(string group)
        {
            Group = group;
            checkBoxGroup = false;
        }

        public Infa() { }

        private bool checkBoxGroup { get; set; }
        public string Group { get; set; }
        public ObservableCollection<InsadeIformation> ListStr { get; set; } = new ObservableCollection<InsadeIformation>();

        public bool CheckBoxGroup { get { return checkBoxGroup; } set { checkBoxGroup = value; OnPropertyChanged("CheckBoxGroup"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    [Serializable]
    public class InsadeIformation : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool checkBox { get; set; }
        public bool CheckBox { get { return checkBox; } set { checkBox = value; OnPropertyChanged("CheckBox"); } }

        public InsadeIformation(string name)
        {
            Name = name;
            checkBox = false;
        }

        public InsadeIformation() { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
