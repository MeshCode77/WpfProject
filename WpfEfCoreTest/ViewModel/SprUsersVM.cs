using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class SprUsersVM : INotifyPropertyChanged
    {
        private readonly TestContext db;

        // команда добавления нового пользователя
        private RelayCommand addCommand;

        // Добавление подразделения
        private RelayCommand addPodr;

        // команда кнопки отмены 
        private RelayCommand cancelUser;

        //RelayCommand editCommand;
        private RelayCommand deleteCommand;

        // команда удаления 
        private RelayCommand deleteUserCommand;

        // команда редактирования пользователя

        private RelayCommand editUserCommand;
        //public static string NewNamePodr { get; set; }

        private string newNamePodr;

        // команда открытия окна для редактирования

        private RelayCommand openEditCommandWnd;

        // конструктор класса
        public SprUsersVM()
        {
            // загружаем данные из бд в локальный кэш
            db = new TestContext();
            db.Users.Load();
            db.Podrs.Load();
            db.Infos.Load();
            db.F111s.Load();
            db.Formulars.Load();

            Users = db.Users.Local.ToObservableCollection();
            Podrs = db.Podrs.Local.ToObservableCollection();
            Infos = db.Infos.Local.ToObservableCollection();
            F111s = db.F111s.Local.ToObservableCollection();
        }

        // свойства для Users
        //public static int Id { get; set; }
        public static string Lname { get; set; }
        public static string Fname { get; set; }
        public static string Mname { get; set; }
        public static Podr IdPodrNavigation { get; set; }

        public static string SelectNamePodr { get; set; }
        public static Podr UserPodr { get; set; }
        public static Info UserInfo { get; set; }


        public static Info IdUserNavigation { get; set; }


        // свойства для Podrs
        public static string NamePodr { get; set; }

        public string NewNamePodr
        {
            get => newNamePodr;
            set
            {
                newNamePodr = value;
                OnPropertyChanged();
            }
        }


        // свойства для Info
        public static string Login { get; set; }
        public static string Pass { get; set; }
        public static string Doljnost { get; set; }
        public static string NameComp { get; set; }
        public static string MAC { get; set; }
        public static string Vtel { get; set; }


        // свойства для выделенных элементов

        public static User SelectedUser { get; set; }
        public static Podr SelectedPodr { get; set; }
        public static Info SelectedInfo { get; set; }

        //[NotNull]
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var resultStr = "";

                    if (Lname == null || Lname.Replace(" ", "").Length == 0) SetRedBlockControll(wnd, "LnameBlock");
                    if (Fname == null || Fname.Replace(" ", "").Length == 0) SetRedBlockControll(wnd, "FnameBlock");
                    if (Mname == null || Mname.Replace(" ", "").Length == 0) SetRedBlockControll(wnd, "LnameBlock");
                    if (Doljnost == null || Doljnost.Replace(" ", "").Length == 0)
                        SetRedBlockControll(wnd, "DoljBlock");
                    if (NameComp == null || NameComp.Replace(" ", "").Length == 0)
                        SetRedBlockControll(wnd, "NameCompBlock");
                    if (IdPodrNavigation == null)
                    {
                        MessageBox.Show("Укажите подразделение");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateUser(Lname, Fname, Mname, Doljnost, NameComp, IdPodrNavigation);
                        UpdateAllDataView();

                        ShowMessageToUser(resultStr); // показать сообщение 
                        SetNullValuesToProperties(); // обнулить свойства
                        wnd.Close();
                    }
                });
            }
        }

        public RelayCommand AddPodr
        {
            get
            {
                return addPodr ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var result = "";

                    if (NewNamePodr == null || NewNamePodr.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "NamePodrBlock");
                    }
                    else
                    {
                        result = DataWorker.CreatePodr(NewNamePodr);
                        //UpdateAllDataView();
                        UpdatePodrView();

                        ShowMessageToUser(result);
                        SetNullValuesToProperties();
                        //wnd.Close();
                    }
                });
            }
        }

        public RelayCommand EditUserCommand
        {
            get
            {
                return editUserCommand ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        var resultStr = "Не выбран сотрудник";
                        var noPodr = "Не выбрано подразделение";


                        if (SelectedUser != null)
                        {
                            if (IdPodrNavigation != null)
                            {
                                resultStr = DataWorker.EditUser(SelectedUser, Lname, Fname, Mname, Doljnost, NameComp,
                                    IdPodrNavigation);

                                //UpdateAllDataView();
                                UpdateAllUsersView();
                                //UpdateAllPodrView();

                                ShowMessageToUser(resultStr);
                                wnd.Close();
                                SetNullValuesToProperties();
                            }
                            else
                            {
                                SetRedBlockControll(wnd, "cbPodr");
                                ShowMessageToUser(noPodr);
                            }
                        }
                        else
                        {
                            ShowMessageToUser(resultStr);
                            SetRedBlockControll(wnd, "LnameBlock");
                        }
                    }
                );
            }
        }

        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedUser != null)
                        {
                            resultStr = DataWorker.DeleteUser(SelectedUser);
                            //UpdateAllDataView();
                            ShowMessageToUser(resultStr);

                            UpdateAllUsersView();

                            //обнуление свойств
                            SetNullValuesToProperties();
                        }
                    }
                );
            }
        }

        public RelayCommand OpenEditCommandWnd
        {
            get
            {
                return openEditCommandWnd ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";

                        if (SelectedUser != null) OpenEditUserWndMethod(SelectedUser);
                    }
                );
            }
        }

        public RelayCommand CancelUser
        {
            get
            {
                return cancelUser ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        SetNullValuesToProperties();
                        wnd.Close();
                    }
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // метод установки окна по центру экрана
        private void SetCentralPositionAndOpen(AddUserWindow newsprUsers)
        {
            //newsprUsers.Owner = Application.Current.MainWindow;
            //newsprUsers.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newsprUsers.ShowDialog();
        }

        // обнуление свойств ViewModel user podr info
        private void SetNullValuesToProperties()
        {
            // для 
            Lname = null;
            Fname = null;
            Mname = null;
            //IdPodrNavigation = null;

            // ДЛЯ Podr
            NamePodr = null;
            Login = null;
            Pass = null;
            MAC = null;
            Doljnost = null;
            NameComp = null;
        }


        private void OpenEditUserWndMethod(User user)
        {
            var editUserWindow = new EditUserWindow(user);
            editUserWindow.ShowDialog();
        }

        // метод валидации текстбоксов
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            var block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }


        private void UpdateAllDataView()
        {
            UpdateAllUsersView();
            //UpdateAllPodrView();
            //UpdateAllInfoView();
        }

        private void UpdateAllUsersView()
        {
            AllUsers = DataWorker.GetAllUsers();
            SprUsers.AllUsersView.ItemsSource = null;
            SprUsers.AllUsersView.Items.Clear();
            SprUsers.AllUsersView.ItemsSource = AllUsers;
            SprUsers.AllUsersView.Items.Refresh();
        }

        private void UpdateAllPodrView()
        {
            AllPodrs = DataWorker.GetAllPodrs();
            SprUsers.AllPodrsView.ItemsSource = null;
            SprUsers.AllPodrsView.Items.Clear();
            SprUsers.AllPodrsView.ItemsSource = AllPodrs;
            SprUsers.AllPodrsView.Items.Refresh();
        }

        private void UpdateAllInfoView()
        {
            AllInfos = DataWorker.GetAllInfos();
            SprUsers.AllInfosView.ItemsSource = null;
            SprUsers.AllInfosView.Items.Clear();
            SprUsers.AllInfosView.ItemsSource = AllPodrs;
            SprUsers.AllInfosView.Items.Refresh();
        }

        private void UpdatePodrView()
        {
            AllPodrs = DataWorker.GetAllPodrs();
            SprPodrWindow.UpdatePodrsView.ItemsSource = null;
            SprPodrWindow.UpdatePodrsView.Items.Clear();
            SprPodrWindow.UpdatePodrsView.ItemsSource = AllPodrs;
            SprPodrWindow.UpdatePodrsView.Items.Refresh();
        }


        private void ShowMessageToUser(string message)
        {
            var messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }

        private void SetCenterPositionAndOpen(Window window)
        {
            //window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Свойства для таблиц БД

        //IEnumerable<Phone> phones;
        private ObservableCollection<User> users;
        private ObservableCollection<Podr> podrs;
        private ObservableCollection<Info> infos;
        private ObservableCollection<F111> f111s;
        private ObservableCollection<Formular> formulars;

        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Podr> Podrs
        {
            get => podrs;
            set
            {
                podrs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Info> Infos
        {
            get => infos;
            set
            {
                infos = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<F111> F111s
        {
            get => f111s;
            set
            {
                f111s = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Formular> Formulars
        {
            get => formulars;
            set
            {
                formulars = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Получить все данные в свойства из БД

        // получить всех пользователей

        private ObservableCollection<User> allUsers = DataWorker.GetAllUsers();

        public ObservableCollection<User> AllUsers
        {
            get => allUsers;
            set
            {
                allUsers = value;
                OnPropertyChanged();
            }
        }


        // получить все подразделения
        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        public ObservableCollection<Podr> AllPodrs
        {
            get => allPodrs;
            set
            {
                allPodrs = value;
                OnPropertyChanged();
            }
        }


        // получить все Info

        private ObservableCollection<Info> allInfos = DataWorker.GetAllInfos();

        public ObservableCollection<Info> AllInfos
        {
            get => allInfos;
            set
            {
                allInfos = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды и методы открытия окон

        // команда открытия окна AddUserWindow
        public RelayCommand openAddUserWnd;

        public RelayCommand OpenAddUserWnd
        {
            get { return openAddUserWnd ?? new RelayCommand(obj => { OpenAddUserWndMethod(); }); }
        }

        // метод открытия окна AddUserWindow
        private void OpenAddUserWndMethod()
        {
            var newAddUsers = new AddUserWindow();
            SetCentralPositionAndOpen(newAddUsers);
        }

        #endregion
    }
}