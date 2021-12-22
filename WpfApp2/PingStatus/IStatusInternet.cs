using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfApp2.BaseCommands;
using WpfApp2.Infomation;
using WpfApp2.PingStatus;


namespace WpfApp2.PingStatus
{
    interface IStatusInternet
    {
        public string Status { get; }
        public ToolTip ToolTip { get; }
        public Infas TreeInfas { get; set; }
        public FlowDocument FlowDocument { get; set; }

        public StatusInternetCommand Open { get; }
        public StatusInternetCommand LoadTable { get; }
        public StatusInternetCommand LoadInformationCommand { get; }
    }
}
