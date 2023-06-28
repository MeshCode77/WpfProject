using Microsoft.EntityFrameworkCore;
using SqlServMvvmApp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public  class SprUsersVM : INotifyPropertyChanged
    {
        TestContext db;

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
        //public static string NewNamePodr { get; set; }

        private string newNamePodr;

        public string NewNamePodr
        {
            get { return newNamePodr; }
            set
            {
                newNamePodr = value;
                OnPropertyChanged("NewNamePodr");
            }
        }


        // свойства для Info
        public static string Doljnost { get; set; }
        public static string NameComp { get; set; }



        // свойства для выделенных элементов

        public static User SelectedUser { get; set; }
        public static Podr SelectedPodr { get; set; }
        public static Info SelectedInfo { get; set; }

        //RelayCommand editCommand;
        RelayCommand deleteCommand;

        #region Свойства для таблиц БД

        //IEnumerable<Phone> phones;
        private ObservableCollection<User> users;
        private ObservableCollection<Podr> podrs;
        private ObservableCollection<Info> infos;
        private ObservableCollection<F111> f111s;
        private ObservableCollection<Formular> formulars;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }

        public ObservableCollection<Podr> Podrs
        {
            get { return podrs; }
            set
            {
                podrs = value;
                OnPropertyChanged("Podrs");
            }
        }

        public ObservableCollection<Info> Infos
        {
            get { return infos; }
            set
            {
                infos = value;
                OnPropertyChanged("Infos");
            }
        }

        public ObservableCollection<F111> F111s
        {
            get { return f111s; }
            set
            {
                f111s = value;
                OnPropertyChanged("F111s");
            }
        }

        public ObservableCollection<Formular> Formulars
        {
            get { return formulars; }
            set
            {
                formulars = value;
                OnPropertyChanged("Formulars");
            }
        }
        #endregion

        #region Получить все данные в свойства из БД

        // получить всех пользователей

        private ObservableCollection<User> allUsers = DataWorker.GetAllUsers();

        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set
            {
                allUsers = value;
                OnPropertyChanged("AllUsers");
            }
        }


        // получить все подразделения
        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        public ObservableCollection<Podr> AllPodrs
        {
            get { return allPodrs;}
            set
            {
                allPodrs = value;
                OnPropertyChanged("AllPodrs");
            }
        }


        // получить все Info

        private ObservableCollection<Info> allInfos = DataWorker.GetAllInfos();

        public ObservableCollection<Info> AllInfos
        {
            get { return allInfos; }
            set
            {
                allInfos = value;
                OnPropertyChanged("AllInfos");
            }
        }

        #endregion

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

        #region Команды и методы открытия окон

        // команда открытия окна AddUserWindow
        public RelayCommand openAddUserWnd;

        public RelayCommand OpenAddUserWnd
        {
            get
            {
                return   openAddUserWnd ?? new RelayCommand(obj =>
                {
                    OpenAddUserWndMethod();
                });
            }
           
        }

        // метод открытия окна AddUserWindow
        private void OpenAddUserWndMethod()
        {
            AddUserWindow newAddUsers = new AddUserWindow();
            SetCentralPositionAndOpen(newAddUsers);
        }
        #endregion

        // метод установки окна по центру экрана
        private void SetCentralPositionAndOpen(AddUserWindow newsprUsers)
        {
            
            //newsprUsers.Owner = Application.Current.MainWindow;
            //newsprUsers.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newsprUsers.ShowDialog();
        }



        // команда добавления нового пользователя
        private RelayCommand addCommand;

        //[NotNull]
        public RelayCommand AddCommand
        {
            get
            { return addCommand ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as  Window;

                    string resultStr = "";

                    if (Lname == null || Lname.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "LnameBlock");
                    }
                    if (Fname == null || Fname.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "FnameBlock");
                    }
                    if (Mname == null || Mname.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "LnameBlock");
                    }
                    if (Doljnost == null || Doljnost.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "DoljBlock");
                    }
                    if (NameComp == null || NameComp.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "NameCompBlock");
                    }
                    if (IdPodrNavigation == null)
                    {
                        MessageBox.Show("Укажите подразделение");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateUser(Lname, Fname, Mname, Doljnost, NameComp, IdPodrNavigation);
                        UpdateAllDataView();

                        ShowMessageToUser(resultStr);  // показать сообщение 
                        SetNullValuesToProperties();   // обнулить свойства
                        wnd.Close();
                    }
                });
            }
        }

        // Добавление подразделения
        private RelayCommand addPodr;

        public RelayCommand AddPodr
        {
            get
            {
                return addPodr ?? new RelayCommand(obj => 
                {
                    Window wnd = obj as Window;

                    string result = "";

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

            Doljnost = null;
            NameComp = null;
        }

        // команда редактирования пользователя

        private RelayCommand editUserCommand;
        public RelayCommand EditUserCommand
        {
            get
            {
                return editUserCommand ?? new RelayCommand(obj =>
                    {
                        Window wnd = obj as Window;

                        string resultStr = "Не выбран сотрудник";
                        string noPodr = "Не выбрано подразделение";


                        if (SelectedUser != null)
                        {
                            if (IdPodrNavigation != null)
                            {
                                resultStr = DataWorker.EditUser(SelectedUser, Lname, Fname, Mname, Doljnost, NameComp, IdPodrNavigation);

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

        // команда удаления 
        private RelayCommand deleteUserCommand;
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return deleteUserCommand ?? new RelayCommand(obj =>
                    {
                        string resultStr = "Ничего не выбрано";
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

        // команда открытия окна для редактирования

        private RelayCommand openEditCommandWnd;
        public RelayCommand OpenEditCommandWnd
        {
            get
            {
                return openEditCommandWnd ?? new RelayCommand(obj =>
                    {
                        string resultStr = "Ничего не выбрано";

                        if (SelectedUser != null)
                        {
                            OpenEditUserWndMethod(SelectedUser);
                        }
                    }
                );
            }
        }

        // команда кнопки отмены 
        private RelayCommand cancelUser;
        public RelayCommand CancelUser
        {
            get
            {
                return cancelUser ?? new RelayCommand(obj =>
                    {
                        Window wnd = obj as Window;

                        SetNullValuesToProperties();
                        wnd.Close();
                    }
                );
            }
        }


        private void OpenEditUserWndMethod(User user)
        {
            EditUserWindow editUserWindow = new EditUserWindow(user);
            editUserWindow.ShowDialog();
        }

        // метод валидации текстбоксов
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
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
            MessageView messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }

        private void SetCenterPositionAndOpen(Window window)
        {
            //window.Owner = Application.Current.MainWindow;
            //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
