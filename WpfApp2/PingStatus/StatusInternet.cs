using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfApp2.BaseCommands;
using WpfApp2.Infomation;
using WpfApp2.PingStatus;
using WpfApp2.ViewModel;

namespace WpfApp2.PingStatus
{
    class CheckStatus
    {
        public static IStatusInternet Ping()
        {
            Ping ping = new Ping();
            try
            {
                PingReply pingReply = ping.Send("google.com");
                return new StatusOnlineViewModel(new StatusOnline());
            }
            catch
            {
                return new StatusOfflineViewModel(new StatusOfline());
            }
        }
    }

    public class StatusOnline : IStatusInternet
    {
        public string Status { get; }
        public ToolTip ToolTip { get; }
        public Infas TreeInfas { get; set; }
        public FlowDocument FlowDocument { get; set; }
        public string Placeholder { get; set; }

        public StatusInternetCommand Open { get; }
        public StatusInternetCommand LoadTable { get; }
        public StatusInternetCommand LoadInformationCommand { get; }

        public StatusOnline()
        {
            
        }
    }

    class StatusOfline : IStatusInternet
    {
        public string Status { get; }
        public ToolTip ToolTip { get; }
        public Infas TreeInfas { get; set; }
        public FlowDocument FlowDocument { get; set; }

        public StatusInternetCommand Open { get; }
        public StatusInternetCommand LoadTable { get; }
        public StatusInternetCommand LoadInformationCommand { get; }

        public StatusOfline()
        {

        }   
    }
}
