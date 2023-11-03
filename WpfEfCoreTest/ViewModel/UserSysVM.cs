using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    internal class UserSysVM : INotifyPropertyChanged
    {
        //public static string LoginLogo { get; set; }
        private static string loginLogo;
        private UserSy _selectedUserSys;

        // команда добавления нового пользователя
        private RelayCommand addSysCommand;

        // получить все подразделения
        private ObservableCollection<UserSy> allUserSys; // = DataWorker.GetAllUserSys();

        // Команда удаления пользователя
        private RelayCommand deleteSysUserCmd;

        private RelayCommand editSysUserCommand;

        // команда открытия окна AddUserWindow
        public RelayCommand openAddSysUserWnd;

        private RelayCommand openEditSysUserCmd;

        private string passLogo;

        private char passLogoChar;

        public RelayCommand sigIn;

        //public string Role { get; set; }


        public UserSysVM()
        {
            allUserSys = DataWorker.GetAllUserSys();

            if (SelectedUserSys != null)
            {
                Fname = SelectedUserSys.Fname;
                Login = SelectedUserSys.Login;
                Pass = SelectedUserSys.Pass;
            }
            else
            {
                Fname = null;
                Login = null;
                Pass = null;
            }
        }

        //public static int Id { get; set; }
        public static string Login { get; set; }
        public static string Pass { get; set; }
        public static string Fname { get; set; }

        public string LoginLogo
        {
            get => loginLogo;
            set
            {
                loginLogo = value;
                OnPropertyChanged(nameof(LoginLogo));
            }
        }

        public string PassLogo
        {
            get => passLogo;
            set
            {
                passLogo = value;
                OnPropertyChanged(nameof(PassLogo));
            }
        }


        public ObservableCollection<UserSy> AllUserSys
        {
            get => allUserSys;
            set
            {
                allUserSys = value;
                OnPropertyChanged(nameof(AllUserSys));
            }
        }


        public static UserSy SelectedUserSys { get; set; }
        //public UserSy SelectedUserSys
        //{
        //    get => _selectedUserSys;
        //    set
        //    {
        //        _selectedUserSys = value;
        //        OnPropertyChanged(nameof(SelectedUserSys));
        //    }
        //}

        #region вход в систему

        public RelayCommand SigIn
        {
            get
            {
                return sigIn ?? new RelayCommand(obj =>
                {
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

                    var result = DataWorker.Registration(LoginLogo, PassLogo);

                    if (result)
                    {
                        var main = new MainWindow();
                        main.Show();

                        if (window != null) window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль не верен.\n\n   Попробуйте еще раз...", "Информация...",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        #endregion

        public RelayCommand OpenAddSysUserWnd
        {
            get
            {
                return openAddSysUserWnd ?? new RelayCommand(obj =>
                {
                    SelectedUserSys = null;
                    var newAddUsers = new AddSysUserViewxaml(SelectedUserSys);
                    newAddUsers.ShowDialog();
                });
            }
            private set => openAddSysUserWnd = value;
        }


        #region Добавляем пользователя

        public RelayCommand AddSysUser
        {
            get
            {
                return addSysCommand ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var resultStr = "Ошибка добавления данных";

                    if (Fname == null || Fname.Replace(" ", "").Length == 0)
                        SetRedBlockControll(wnd, "FnameBlock");

                    if (Login == null || Login.Replace(" ", "").Length == 0)
                        SetRedBlockControll(wnd, "LoginBlock");

                    if (Pass == null || Pass.Replace(" ", "").Length == 0)
                        SetRedBlockControll(wnd, "PassBlock");

                    if ((Fname != null) & (Login != null) & (Pass != null))
                    {
                        resultStr = DataWorker.CreateSysUser(Fname, Login, Pass);

                        UpdateAllSysUsersView();

                        ShowMessageToUser(resultStr); // показать сообщение 
                        SetNullValuesToProperties(); // обнулить свойства
                        wnd.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка", "Не все поля заполнены", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                });
            }
        }

        #endregion

        // Команда открытия окна для редактирования сис пользователя
        public RelayCommand OpenEditSysUserCmd
        {
            get
            {
                return openEditSysUserCmd ?? new RelayCommand(obj =>
                    {
                        if (SelectedUserSys != null)
                        {
                            var edSysUs = new EditSysUser(SelectedUserSys);
                            edSysUs.ShowDialog();
                        }
                    }
                );
            }
        }

        // Редактирование сис пользователя
        public RelayCommand EditSysUserCmd
        {
            get
            {
                return editSysUserCommand ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        var resultStr = "Не выбран пользователь системы";

                        if (SelectedUserSys != null)
                        {
                            resultStr = DataWorker.EditSysUser(SelectedUserSys, Fname, Login, Pass);

                            UpdateAllSysUsersView();

                            ShowMessageToUser(resultStr);
                            wnd.Close();
                            SetNullValuesToProperties();
                        }
                        else
                        {
                            ShowMessageToUser(resultStr);
                            SetRedBlockControll(wnd, "FnameBlock");
                        }
                    }
                );
            }
        }

        // Удаление сис пользователя
        public RelayCommand DeleteSysUserCmd
        {
            get
            {
                return deleteSysUserCmd ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";

                        // ПРОВЕРКА ПЕРЕД УДАЛЕНИЕМ
                        var result = MessageBox.Show("Вы уверены,что хотите удалить этого пользователя",
                            "В Н И М А Н И Е ! ! !",
                            MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.OK)
                        {
                            if (SelectedUserSys != null) resultStr = DataWorker.DeleteSysUser(SelectedUserSys);

                            // обновление
                            UpdateAllSysUsersView();
                            // обнуление свойств
                            SetNullValuesToProperties();
                            // вывод сообщения
                            ShowMessageToUser(resultStr);
                        }
                    }
                );
            }
        }


        public ObservableCollection<UserSy> AllUsersys
        {
            get => allUserSys;
            set
            {
                allUserSys = value;
                OnPropertyChanged(nameof(AllUsersys));
            }
        }


        private void UpdateAllSysUsersView()
        {
            AllUsersys = DataWorker.GetAllUserSys();
            UserManagerView.UpdateSysUserView.ItemsSource = null;
            UserManagerView.UpdateSysUserView.Items.Clear();
            UserManagerView.UpdateSysUserView.ItemsSource = AllUserSys;
            UserManagerView.UpdateSysUserView.Items.Refresh();
        }


        private void OpenEditUserSysWnd(UserSy selectedUserSys)
        {
            var edSysUs = new EditSysUser(selectedUserSys);
            edSysUs.ShowDialog();
        }


        // метод валидации текстбоксов
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            var block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
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

        // обнуление свойств ViewModel user podr info
        private void SetNullValuesToProperties()
        {
            // для 
            Fname = null;
            Login = null;
            Pass = null;
        }


        #region реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}