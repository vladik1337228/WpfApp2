using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Serialization;
using WpfApp2.BaseCommands;
using WpfApp2.Infomation;
using WpfApp2.PingStatus;

namespace WpfApp2.ViewModel
{
    class StatusOfflineViewModel : INotifyPropertyChanged, IStatusInternet
    {
        public string Status { get { return "Offline"; } }
        public ToolTip ToolTip { get { return new ToolTip() { Content = $"{Status} - Данные неактуальны" }; } }
        public Infas TreeInfas { get { return statusOfline.TreeInfas; } set { statusOfline.TreeInfas = value; OnPropertyChanged("TreeInfas"); } }
        public FlowDocument FlowDocument { get { return statusOfline.FlowDocument; } set { statusOfline.FlowDocument = value; OnPropertyChanged("FlowDocument"); } }

        private StatusInternetCommand open;
        public StatusInternetCommand Open { get { return open ?? (open = new StatusInternetCommand(obj => 
        {
            MessageBox.Show("Offile не позволяет менять настройки!", "Error");
        })); } }

        public StatusInternetCommand loadTable;
        public StatusInternetCommand LoadTable { get { return loadTable ?? (loadTable = new StatusInternetCommand(obj => 
        {
            Type type = typeof(InsadeIformation);
            if (obj.GetType() == typeof(InsadeIformation))
            {
                InsadeIformation infa = (InsadeIformation)obj;

                if (File.Exists($"Table\\{infa.Name}.xaml"))
                {
                    using (FileStream fs = File.Open($"Table\\{infa.Name}.xaml", FileMode.Open, FileAccess.Read))
                    {
                        FlowDocument.Blocks.Clear();
                        FlowDocument = (FlowDocument)XamlReader.Load(fs);
                    }
                }
                else
                    MessageBox.Show("Данная таблица не была загружена!", "Error");
            }

        })); } }


        private StatusInternetCommand loadInternetCommand;
        public StatusInternetCommand LoadInformationCommand
        {
            get
            {
                return loadInternetCommand ??
                    (loadInternetCommand = new StatusInternetCommand
                    (obj =>
                    {
                        XmlSerializer formatter = new XmlSerializer(typeof(Infas));
                        Infas infas;
                        using (FileStream fs = new FileStream(@"TreeView\treeview.xml", FileMode.OpenOrCreate))
                        {
                            infas = (Infas)formatter.Deserialize(fs);
                        }

                        foreach (var item in infas.InfaList)
                            TreeInfas.InfaList.Add(item);
                    })
                    );
            }
        }

        private StatusOfline statusOfline;

        public StatusOfflineViewModel(StatusOfline statusOfline)
        {
            this.statusOfline = statusOfline;
            TreeInfas = new Infas();
            FlowDocument = new FlowDocument();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
