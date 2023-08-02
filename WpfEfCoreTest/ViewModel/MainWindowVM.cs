using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private const string Msg1 = "true";
        private const string Msg2 = "false";
        public static TestContext db;
        public static User _selectedUser;
        public static F111 _selectedF111;


        //private static CheckBox cb = Cbox;

        private readonly ObservableCollection<bool> getCheckBoxRemontIsChecked = new();

        private bool _isSelected; // { get; set; } //= DataWorker.GetAllDataF111();

        private Podr _selectedPodr;

        private F111 _selectedRow;

        private ObservableCollection<F111> allDataF111; // = DataWorker.GetAllDataF111(_selectedUser.UserF111.IdUser);


        // получить все подразделения
        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        private RelayCommand openNetworkScan;


        // команда открытия окна карточки Ф111
        public RelayCommand openWindowF111;

        // команда открытия окна формуляра
        public RelayCommand openWindowFormular;

        // команда открытия окна Otchet
        public RelayCommand openWndOtchetAllOborud;

        private RelayCommand openWndOtchetRemont;

        private RelayCommand openWndUserManagerCmd;

        private RelayCommand selectItemCommand;

        private TestContext tc;
        private RelayCommand titleCancel;
        private RelayCommand titleComplCancel;

        private RelayCommand titleComplOK;

        private RelayCommand titleOK;

        //конструктор
        public MainWindowVM()
        {
            db = new TestContext();

            //db.Podrs.Load();
            //db.Podrs.ToObservableCollection();
            //podrs = new ObservableCollection<Podr>();

            AllPodrs = DataWorker.GetAllPodrs();


            //db.Users.Load();
            //db.Users.ToObservableCollection();
            users = new ObservableCollection<User>();


            infos = new ObservableCollection<Info>();

            //db.F111s.Load();
            //db.F111s.ToObservableCollection();
            f111s = new ObservableCollection<F111>();


            //db.Formulars.Load();
            //db.Formulars.ToObservableCollection();
            formular = new ObservableCollection<Formular>();

            //ToggleCheckBox();
        }


        // команда выбора столбца Ремонт и выбора строки (обьекта) в F111
        public RelayCommand SelectItemCommand
        {
            get
            {
                return selectItemCommand ?? new RelayCommand(obj =>
                {
                    SelectedF111 = (F111)obj; // установка курсора выбора строки на строку туда где установлен checkBox

                    if (SelectedF111.Remont) // если значение Remont = true это значение такое как из базы
                    {
                        var tr = new TitleRemontView(); // окно для ввода описания неисправности
                        tr.ShowDialog();

                        if (tr.DialogResult == true)
                            return;

                        FilteredF111s.Clear();
                        FilteredF111s = DataWorker.GetAllDataF111(SelectedUser.Id);
                    }
                    else
                    {
                        var trc = new TitleRemontCompleted();
                        var res = trc.ShowDialog();

                        if (res == true)
                        {
                            DataWorker.RemoveToRemont(SelectedF111.Id);

                            FilteredF111s.Clear();
                            FilteredF111s = DataWorker.GetAllDataF111(SelectedUser.Id);

                            //return;
                        }
                        else if (res == false)
                        {
                            //DataWorker.RemoveToRemont(SelectedF111.Id);

                            FilteredF111s.Clear();
                            FilteredF111s = DataWorker.GetAllDataF111(SelectedUser.Id);
                        }
                    }
                });
            }
        }


        private ObservableCollection<Podr> podrs { get; }
        private ObservableCollection<User> users { get; set; }
        private ObservableCollection<Info> infos { get; set; }
        public static ObservableCollection<F111> f111s { get; set; }
        public static ObservableCollection<Formular> formular { get; set; }

        public ObservableCollection<Podr> AllPodrs
        {
            get => allPodrs;
            set
            {
                allPodrs = value;
                OnPropertyChanged();
            }
        }

        public Podr SelectedPodr
        {
            get => _selectedPodr;

            set
            {
                _selectedPodr = value;
                OnPropertyChanged(nameof(SelectedPodr));

                users.Clear();
                f111s.Clear();

                if (_selectedPodr != null)
                    db.Users.Where(u => u.IdPodr == _selectedPodr.Id).ToObservableCollection()
                        .Foreach(x => users.Add(x));
            }
        }


        public ObservableCollection<User> FilteredUsers
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(FilteredUsers));
            }
        }


        // Выбор пользователя и отображения связанных данных в F111
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                if (_selectedUser != null)
                {
                    DataTransfer.IdUser =
                        _selectedUser
                            .Id; // передача выбраного id пользователя в DataTranfer для использ в других классах

                    //FilteredF111s =
                    //    new ObservableCollection<F111>(); // создаем новую коллекцию F111 - это решает проблему неработающего SelectedUser для F111

                    if (f111s != null)
                    {
                        f111s.Clear();

                        //if (db.F111s == null)
                        //    return;

                        db.F111s.Where(u => u.IdUser == SelectedUser.Id).ToObservableCollection()
                            .Foreach(x => f111s.Add(x));
                    }


                    infos.Clear();
                    db.Infos.Where(u => u.IdUser == SelectedUser.Id).ToObservableCollection()
                        .Foreach(x => infos.Add(x));
                }
            }
        }

        public ObservableCollection<Info> FilteredInfos
        {
            get => infos;
            set
            {
                infos = value;
                OnPropertyChanged(nameof(FilteredInfos));
            }
        }

        public ObservableCollection<F111> FilteredF111s
        {
            get => f111s;
            set
            {
                f111s = value;
                OnPropertyChanged(nameof(FilteredF111s));
                //GetAllCheckBoxToRemont = DataWorker.GetAllDataF111ToId(SelectedUser.Id);
            }
        }


        public ObservableCollection<Formular> FilteredFormular
        {
            get => formular;
            set
            {
                formular = value;
                OnPropertyChanged(nameof(FilteredFormular));

                if (formular != null)
                {
                    formular.Clear();

                    if (SelectedF111 != null)
                        db.Formulars.Where(u => u.Idf111 == _selectedF111.Id).ToObservableCollection()
                            .Foreach(x => formular.Add(x));
                }
            }
        }

        // выбор строки в F111
        public F111 SelectedF111
        {
            get => _selectedF111;

            set
            {
                _selectedF111 = value;
                OnPropertyChanged(nameof(SelectedF111));

                if (SelectedF111 != null)
                {
                    DataTransfer.IdF111 = _selectedF111.Id; // передача IdF111 для других классов

                    //FilteredFormular = new ObservableCollection<Formular>();

                    formular.Clear();

                    if (db.Formulars == null)
                        return;
                    db.Formulars.Where(u => u.Idf111 == DataTransfer.IdF111).ToObservableCollection()
                        .Foreach(x => formular.Add(x));

                    FormularVM.Idf111 = _selectedF111.Id; // связывание таблиц F111 и Formular при пустом формуляре
                }
            }
        }

        // Команда открытия окна F111View
        public RelayCommand OpenWindowF111
        {
            get
            {
                return openWindowF111 ?? new RelayCommand(obj =>
                {
                    _selectedUser = obj as User;
                    //_selectedF111 = obj as F111;
                    var f111View = new F111View();
                    f111View.ShowDialog();
                });
            }
        }

        // Команда открытия окна F111View
        public RelayCommand OpenWindowFormular
        {
            get
            {
                return openWindowFormular ?? new RelayCommand(obj =>
                {
                    _selectedF111 = obj as F111;
                    var formular = new FormularView();
                    formular.ShowDialog();
                });
            }
        }

        public ObservableCollection<F111> AllDataF111
        {
            get => allDataF111;
            set
            {
                allDataF111 = value;
                OnPropertyChanged(nameof(AllDataF111));
            }
        }

        // Команда открытия окна OtchetAllOborud
        public RelayCommand OpenWndOtchetAllOborud
        {
            get
            {
                return openWndOtchetAllOborud ?? new RelayCommand(obj =>
                {
                    //_selectedUser = obj as User;
                    //_selectedF111 = obj as F111;
                    var otchetAllObView = new OtchetAllOborud();
                    otchetAllObView.ShowDialog();
                });
            }
        }

        // Команда открытия окна UserManagerView
        public RelayCommand OpenWndUserManagerCmd
        {
            get
            {
                return openWndUserManagerCmd ?? new RelayCommand(obj =>
                {
                    var userMan = new UserManagerView();
                    userMan.ShowDialog();
                });
            }
        }

        // Команда открытия окна OtchetRemontView
        public RelayCommand OpenWndOtchetRemont
        {
            get
            {
                return openWndOtchetRemont ?? new RelayCommand(obj =>
                {
                    var otchRem = new OtchetRemontView();
                    otchRem.ShowDialog();
                });
            }
        }

        // Команда открытия окна OtchetRemontView
        public RelayCommand OpenNetworkScan
        {
            get
            {
                return openNetworkScan ?? new RelayCommand(obj =>
                {
                    var ns = new NetScanView();
                    ns.Show();
                });
            }
        }


        public void UpdateF111ToUser()
        {
            var result = "Данные не изменены";

            FilteredF111s = DataWorker.GetAllDataF111(DataTransfer.IdUser);

            MainWindow.AllDataF111ToUser.Items.Refresh();

            MainWindow.AllDataF111ToUser.ItemsSource = FilteredF111s;
        }


        public void UpdateFormular()
        {
            MainWindow.AllDataF111ToUser.ItemsSource =
                FilteredF111s; // это решает проблему отображения данных в контроле 

            FilteredFormular = DataWorker.GetAllDataFormularToIdF111(DataTransfer.IdF111);
            MainWindow.AllDataFormular.Items.Refresh();
            MainWindow.AllDataFormular.ItemsSource = FilteredFormular;
        }

        public static void RefreshF111()
        {
            f111s = DataWorker.GetAllDataF111(DataTransfer.IdUser);

            MainWindow.AllDataF111ToUser.Items.Refresh();

            MainWindow.AllDataF111ToUser.ItemsSource = f111s;
        }


        #region КОМАНДЫ ДЛЯ ОПИСАНИЯ ПРЕДВАРИТЕЛЬНЫХ И РЕАЛЬНЫХ ПРИЧИН НЕИСПРАВНОСТЕЙ ОБОРУДОВАНИЯ

        public RelayCommand TitleOK
        {
            get
            {
                return titleOK ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var otchRem = new OtchetRemontVM();

                    if (Title != null) // проверка на введенность информации о неисправности
                    {
                        otchRem.AddToRemont(SelectedUser, SelectedF111, Title);
                    }
                    else
                    {
                        MessageBox.Show("Введите неисправность оборудования", "!!! ВНИМАНИЕ !!!");
                        return;
                    }


                    if (wnd != null) wnd.DialogResult = true;
                });
            }
        }

        public RelayCommand TitleCancel
        {
            get
            {
                return titleCancel ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    FilteredF111s.Clear();
                    DataWorker.GetAllDataF111();

                    if (wnd != null) wnd.DialogResult = false;
                });
            }
        }


        public RelayCommand TitleComplOK
        {
            get
            {
                return titleComplOK ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var otchRem = new OtchetRemontVM();

                    if (TitleCompleted != null) // проверка на введенность информации о неисправности
                    {
                        otchRem.AddToRemontCompleted(SelectedUser, SelectedF111, TitleCompleted);
                    }
                    else
                    {
                        MessageBox.Show("Введите реальную неисправность оборудования", "!!! ВНИМАНИЕ !!!");
                        return;
                    }

                    if (wnd != null) wnd.DialogResult = true;
                });
            }
        }

        public RelayCommand TitleComplCancel
        {
            get
            {
                return titleComplCancel ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    FilteredF111s.Clear();
                    DataWorker.GetAllDataF111();

                    if (wnd != null) wnd.DialogResult = false;
                });
            }
        }

        #endregion

        #region Свойства таблицы F111

        private string model;

        public string Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        private string kartNum;

        public string KartNum
        {
            get => kartNum;
            set
            {
                kartNum = value;
                OnPropertyChanged(nameof(KartNum));
            }
        }

        private string numForm;

        public string NumForm
        {
            get => numForm;
            set
            {
                numForm = value;
                OnPropertyChanged(nameof(NumForm));
            }
        }


        public static string invNum;

        public string InvNum
        {
            get => invNum;
            set
            {
                invNum = value;
                OnPropertyChanged(nameof(InvNum));
            }
        }


        private string zavodNum;

        public string ZavodNum
        {
            get => zavodNum;
            set
            {
                zavodNum = value;
                OnPropertyChanged(nameof(ZavodNum));
            }
        }


        private DateTime gtDate;

        public DateTime GtDate
        {
            get => gtDate;
            set
            {
                gtDate = value;
                OnPropertyChanged(nameof(GtDate));
            }
        }


        private DateTime? outDate;

        public DateTime? OutDate
        {
            get => outDate;
            set
            {
                outDate = value;
                OnPropertyChanged(nameof(OutDate));
            }
        }


        private bool remont;

        public bool Remont
        {
            get => remont;

            set
            {
                remont = value;
                OnPropertyChanged(nameof(Remont));
            }
        }

        private string title;

        public string Title
        {
            get => title;

            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string titleCompleted;

        public string TitleCompleted
        {
            get => titleCompleted;

            set
            {
                titleCompleted = value;
                OnPropertyChanged(nameof(TitleCompleted));
            }
        }


        //public static CheckBox Cbox { get; private set; }

        //public static bool? Spisan { get; set; }

        #endregion


        #region Реализация интерфейса   INotyfyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}