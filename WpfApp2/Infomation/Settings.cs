using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace WpfApp2.Infomation
{
    class Settings
    {
        public string StateDrop { get; set; }
        public string Address { get; set; }
        public string DragAndDrop { get; set; }
        public string Placeholder { get; set; }
        public string PalceholderRichText { get; set; }
        public string PalceholderAppName { get; set; }
        public bool EnabledButton { get; set; }

        public Settings()
        {

        }
    }
}
