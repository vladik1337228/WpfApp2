using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.BaseCommands;
using WpfApp2.Infomation;
using System.Windows.Documents;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private Settings settings;

        public SettingsViewModel(Settings settings)
        {
            this.settings = settings;
            EnabledButton = true;
        }

        public void SetInfa()
        {
            if (String.IsNullOrEmpty(PalceholderRichText))
                PalceholderRichText = "Input your gropus";

            if (String.IsNullOrEmpty(Placeholder))
                Placeholder = "Adsress Google SpreadSheet";

            if (String.IsNullOrEmpty(PalceholderAppName))
                PalceholderAppName = "Application Name";

            StateDrop = "Перетащите файл Secret";
        }

        public string Address { get { return settings.Address; } set { settings.Address = value; OnPropertyChanged("Address");  } }
        public string DragAndDrop { get { return settings.DragAndDrop; } set { settings.DragAndDrop = value; OnPropertyChanged("DragAndDrop"); } }
        public string StateDrop { get { return settings.StateDrop; } set { settings.StateDrop = value; OnPropertyChanged("StateDrop"); } }
        public string Placeholder { get { return settings.Placeholder; } set { settings.Placeholder = value; OnPropertyChanged("Placeholder"); } }
        public string PalceholderRichText { get { return settings.PalceholderRichText; } set { settings.PalceholderRichText = value; OnPropertyChanged("PalceholderRichText"); } }
        public string PalceholderAppName { get{ return settings.PalceholderAppName; } set { settings.PalceholderAppName = value; OnPropertyChanged("PalceholderAppName"); } }
        public bool EnabledButton { get { return settings.EnabledButton; } set { settings.EnabledButton = value; OnPropertyChanged("EnabledButton"); } }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private StatusInternetCommand placeholderOn;
        public StatusInternetCommand PlaceholderOn
        {
            get
            {
                return placeholderOn ??
                    (
                        placeholderOn = new StatusInternetCommand
                        (obj =>
                        {
                            string tag = obj.ToString();
                            switch(tag)
                            {
                                case "1":
                                    if (String.IsNullOrEmpty(Placeholder))
                                        Placeholder = "Adsress Google SpreadSheet";
                                    break;
                                case "2":
                                    if (String.IsNullOrEmpty(PalceholderAppName))
                                        PalceholderAppName = "Application Name";
                                    break;
                            }
                        })
                    );
            }
        }

        private StatusInternetCommand placeholderOff;
        public StatusInternetCommand PlaceholderOff
        {
            get
            {
                return placeholderOff ??
                    (
                        placeholderOff = new StatusInternetCommand
                        (obj =>
                        {
                            string tag = obj.ToString();
                            switch (tag)
                            {
                                case "1":
                                    if (Placeholder == "Adsress Google SpreadSheet")
                                        Placeholder = "";
                                    break;
                                case "2":
                                    if (PalceholderAppName == "Application Name")
                                        PalceholderAppName = "";
                                    break;
                            }
                        })
                    );
            }
        }

        private StatusInternetCommand placeholderRichOn;
        public StatusInternetCommand PlaceholderRichOn
        {
            get
            {
                return placeholderRichOn ??
                    (
                        placeholderRichOn = new StatusInternetCommand
                        (obj =>
                        {
                            if (String.IsNullOrEmpty(PalceholderRichText))
                                PalceholderRichText = "Input your groups";
                        })
                    );
            }
        }

        private StatusInternetCommand placeholderRichOff;
        public StatusInternetCommand PlaceholderRichOff
        {
            get
            {
                return placeholderRichOff ??
                    (
                        placeholderRichOff = new StatusInternetCommand
                        (obj =>
                        {
                            if (PalceholderRichText == "Input your groups")
                                PalceholderRichText = "";
                        })
                    );
            }
        }

        private StatusInternetCommand save;
        public StatusInternetCommand Save
        {
            get
            {
                return save ??
                    (
                        save = new StatusInternetCommand
                        (obj =>
                        {
                            MethosSave();
                        })
                    );
            }
        }

        public async void MethosSave()
        {
            EnabledButton = false;
            string[] list = PalceholderRichText.Split(new char[] { '\n', '\r' });

            await Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(@"Settings\Address.txt", false, Encoding.Default))
                {
                    sw.WriteLine(Placeholder);
                    sw.WriteLine(PalceholderAppName.Trim());
                }
                using (StreamWriter sw = new StreamWriter(@"Settings\Group.txt", false, Encoding.Default))
                {
                    foreach (var item in list)
                        if(!string.IsNullOrEmpty(item))
                            sw.WriteLine(item);

                    System.Threading.Thread.Sleep(1000);
                }
            });

            EnabledButton = true;

        }

        private StatusInternetCommand load;
        public StatusInternetCommand Load
        {
            get
            {
                return load ??
                    (
                        load = new StatusInternetCommand
                        (obj =>
                        {
                            List<string> list = new List<string>();

                            using (StreamReader sr = new StreamReader(@"Settings\Address.txt", Encoding.Default))
                            {
                                Placeholder = sr.ReadLine();
                                PalceholderAppName = sr.ReadLine();
                            }
                            using (StreamReader sr = new StreamReader(@"Settings\Group.txt", Encoding.Default))
                            {
                                string line = "";
                                while ((line = sr.ReadLine()) != null)
                                    PalceholderRichText += line + Environment.NewLine;
                            }

                            SetInfa();
                        })
                    );
            }
        }

        private StatusInternetCommand dragDrop;
        public StatusInternetCommand DragDrop
        {
            get
            {
                return dragDrop ??
                    (
                        dragDrop = new StatusInternetCommand
                        (obj =>
                        {
                            DragEventArgs e = (DragEventArgs)obj;
                            string[] filepath = (string[])e.Data.GetData(DataFormats.FileDrop);
                            if (filepath[0].IndexOf(".json") != -1)
                            {
                                FileInfo fileInfo = new FileInfo(@"Settings\Secret\sheets.json");

                                string key = File.ReadAllText(filepath[0]);

                                File.WriteAllText(fileInfo.FullName, key);

                                StateDrop = "Файл успешно добавлен";
                            }
                            else
                                MessageBox.Show("Файл secret должен быть формата *.json!", "Error", MessageBoxButton.OK);
                        })
                    );
            }
        }


    }
}
