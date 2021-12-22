using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using WpfApp2.Infomation;
using WpfApp2.BaseCommands;
using WpfApp2.PingStatus;
using System.Linq;
using System.Windows.Documents;
using System.Runtime.CompilerServices;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4.Data;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Xml.Serialization;
using System.Windows.Interactivity;

namespace WpfApp2.ViewModel
{
    public class StatusOnlineViewModel : Behavior<TreeView>, IStatusInternet, INotifyPropertyChanged
    {
        private StatusInternetCommand loadInternetCommand;
        public StatusInternetCommand LoadInformationCommand
        {
            get
            {
                return loadInternetCommand ??
                    (loadInternetCommand = new StatusInternetCommand
                    (obj =>
                    {
                        using (StreamReader sr = new StreamReader(@"Settings\Group.txt", System.Text.Encoding.Default))
                        {
                            string line = "";
                            while ((line = sr.ReadLine()) != null && line != "")
                            {
                                TreeInfas.InfaList.Add(new Infa(line));

                                foreach (var item in Sheets)
                                {
                                    if (item.Properties.Title.IndexOf(TreeInfas.InfaList.Last().Group) > -1)
                                        TreeInfas.InfaList.Last().ListStr.Add(new InsadeIformation(item.Properties.Title));
                                }
                            }
                        }

                        XmlSerializer formatter = new XmlSerializer(typeof(Infas));

                        using (FileStream fs = File.Open(@"TreeView\treeview.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete))
                        {
                            formatter.Serialize(fs, TreeInfas);
                        }

                    })
                    );
            }
        }

        private StatusInternetCommand loadTable { get; set; }
        public StatusInternetCommand LoadTable 
        { get 
            {
                return loadTable ?? 
                    (loadTable = new StatusInternetCommand(obj => 
                    {
                        Type type = typeof(InsadeIformation);
                        if (obj != null)
                        {
                            ReadTable(obj);
                        }
                    }, x => x is InsadeIformation)
                    );
            } 
        }


        private StatusInternetCommand search;
        public StatusInternetCommand Search
        {
            get
            {
                return search ??
                    (
                        search = new StatusInternetCommand
                        (obj =>
                        {
                            FlowDocument.Blocks.Clear();
                            var list = TreeInfas.InfaList.SelectMany(x => x.ListStr.Where(x => x.CheckBox == true));
                            foreach (var item in list)
                            {
                                ReadTable(item, true);
                            }
                        }, CanSearch)
                    );
            }
        }

        public bool CanSearch(object obj)
        {
            if (String.IsNullOrEmpty(Placeholder) || Placeholder == "Search / Tables" || TreeInfas.InfaList.SelectMany(x => x.ListStr.Where(x => x.CheckBox == true)).Count() < 1)
            {
                MessageBox.Show("Ошибка!");
                return false;
            }
            else
                return true;
        }

        private List<List<object>> SearchValue(IEnumerable<List<object>> value, string NameTable)
        {
            List<List<object>> list = new List<List<object>>();
            list.Add(new List<object>() { "Информация из таблицы -" + NameTable });
            foreach (var item in value)
            {
                foreach (var item1 in item)
                {
                    if (item1.ToString().Contains(Placeholder) || item.Count == 1)
                    {
                        list.Add(item);
                        break;
                    }    
                }
            }
            return list;
        }

        private IList<IList<object>> TableList(InsadeIformation infa)
        {
            var range = $"{infa.Name}!A:Z";
            var request = sheetsService.Spreadsheets.Values.Get(Id, range);
            var respons = request.Execute();
            return respons.Values;
        }

        private void ReadTable(object obj, bool Search = false)
        {
            InsadeIformation infa = (InsadeIformation)obj;
            var values = TableList(infa);

            IEnumerable<List<object>> list123 = null;

            if (values != null)
                list123 = values.Select(x => x.ToList()).Select(x => x.ToList()).Select(x => x.FindAll(y => y.ToString().Trim().Length > 0));
            
            if (Search)
                list123 = SearchValue(list123, infa.Name);

            if (list123 != null)
            {
                int maxCountCell = list123.Max(x => x.Count);

                Table table = new Table();
                TableRowGroup tableRowGroup = new TableRowGroup();
                table.RowGroups.Add(tableRowGroup);

                bool colorRow = true;

                foreach (var item in list123)
                {
                    TableRow tableRow = new TableRow();

                    if (colorRow)
                    {
                        tableRow.Background = Brushes.LightGray;
                        colorRow = false;
                    }
                    else
                    {
                        tableRow.Background = Brushes.Gray;
                        colorRow = true;
                    }

                    if (item.Count == 1)
                        tableRow.Background = Brushes.Coral;


                    //add what&&

                    tableRow.FontFamily = new FontFamily("Consolas");

                    foreach (var item1 in item)
                    {
                        if (item.Count == 1)
                            tableRow.Cells.Add(new TableCell(new Paragraph(new Run(item1.ToString()))) { TextAlignment = TextAlignment.Center, ColumnSpan = maxCountCell });
                        else
                            tableRow.Cells.Add(new TableCell(new Paragraph(new Run(item1.ToString()))) { BorderBrush = Brushes.White, BorderThickness = new Thickness(2, 0, 0, 0) });
                    }

                    tableRowGroup.Rows.Add(tableRow);
                }

                table.CellSpacing = 0;
                if(!Search)
                    FlowDocument.Blocks.Clear();

                FlowDocument.Blocks.Add(table);

                using (FileStream fs = File.Open($"Table\\{infa.Name}.xaml", FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                {
                    XamlWriter.Save(FlowDocument, fs);
                }
            }
            else if(!Search)
                MessageBox.Show("Таблица пуста");
        }

        private StatusInternetCommand open;
        public StatusInternetCommand Open
        {
            get
            {
                return open ??
                    (
                        open = new StatusInternetCommand
                        (obj =>
                        {
                            switch (obj.ToString())
                            {
                                case "WindowSettings":
                                    new WindowSettings().Show();
                                    break;
                                default:
                                    break;
                            }

                            
                        })
                    );
            }
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
                            if (String.IsNullOrEmpty(Placeholder))
                                Placeholder = "Search / Tables";
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
                            if (Placeholder == "Search / Tables")
                                Placeholder = "";
                        })
                    );
            }
        }

        private StatusInternetCommand checkGroup;
        public StatusInternetCommand CheckGroup
        {
            get
            {
                return checkGroup ??
                    (
                        checkGroup = new StatusInternetCommand
                        (obj =>
                        {
                            Infa infa = obj as Infa;
                            foreach (var item in TreeInfas.InfaList.First(x => x.Group == infa.Group).ListStr)
                            {
                                item.CheckBox = infa.CheckBoxGroup;
                            }
                        })
                    );
            }
        }

        private StatusOnline statusOnline;

        public StatusOnlineViewModel(StatusOnline online)
        {
            statusOnline = online;
            TreeInfas = new Infas();
            FlowDocument = new FlowDocument();
            Placeholder = "Search / Tables";
        }

        public StatusOnlineViewModel()
        { }

        static string[] scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName1 = "AppSheets";
        static string Id = "1jmrZK-y666Hywmt7p9mYguGzUGNNU_JYN5oY2DTm2ss";
        static IList<Sheet> Sheets;
        static SheetsService sheetsService;

        static StatusOnlineViewModel()
        {
            GoogleCredential googleCredential;
            using (FileStream fs = new FileStream(@"Settings\Secret\sheets.json", FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(fs).CreateScoped(scopes);
            }

            sheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = ApplicationName1,
            });

            var dataTable = sheetsService.Spreadsheets.Get(Id).Execute();
            Sheets = dataTable.Sheets;

            DirectoryInfo directoryInfo = new DirectoryInfo("Table");

            var list = directoryInfo.GetFiles().Select(x => x.Name).Except(Sheets.Select(x => x.Properties.Title + ".xaml"));
            foreach (var item in list)
                File.Delete(@"Table\" + item);
        }

        public string Status { get { return "Online"; } }
        public ToolTip ToolTip { get { return new ToolTip() { Content = $"{Status} - Данные актуальны" }; } }
        public Infas TreeInfas { get { return statusOnline.TreeInfas; } set { statusOnline.TreeInfas = value; OnPropertyChanged("TreeInfas"); } }
        public FlowDocument FlowDocument { get { return statusOnline.FlowDocument; } set { statusOnline.FlowDocument = value; OnPropertyChanged("FlowDocument"); } }
        public string Placeholder { get { return statusOnline.Placeholder; } set { statusOnline.Placeholder = value; OnPropertyChanged("Placeholder"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
