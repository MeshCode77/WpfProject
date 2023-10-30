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
        // команда добавления нового пользователя
        private RelayCommand addSysCommand;

        // получить все подразделения
        private ObservableCollection<UserSy> allUserSys; // = DataWorker.GetAllUserSys();

        // команда открытия окна AddUserWindow
        public RelayCommand openAddSysUserWnd;
        private UserSy selectedUserSys;

        public RelayCommand sigIn;

        public int Id { get; set; }

        //public int? IdUser { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Fname { get; set; }

        //public string Role { get; set; }


        public ObservableCollection<UserSy> AllUserSys
        {
            get => allUserSys;
            set
            {
                allUserSys = value;
                OnPropertyChanged(nameof(AllUserSys));
            }
        }

        public UserSy SelectedUserSys
        {
            get => selectedUserSys;
            set
            {
                selectedUserSys = value;
                OnPropertyChanged(nameof(SelectedUserSys));
            }
        }

        public RelayCommand SigIn
        {
            get
            {
                return sigIn ?? new RelayCommand(obj =>
                {
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

                    var main = new MainWindow();
                    main.Show();

                    if (window != null) window.Close();
                });
            }
        }

        public RelayCommand OpenAddSysUserWnd
        {
            get
            {
                return openAddSysUserWnd ?? new RelayCommand(obj =>
                {
                    var newAddUsers = new AddSysUserViewxaml();
                    newAddUsers.ShowDialog();
                });
            }
        }

        //Добавляем пользователя
        public RelayCommand AddSysUser
        {
            get
            {
                return addSysCommand ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var resultStr = "";

                    if (Fname == null || Fname.Replace(" ", "").Length == 0) SetRedBlockControll(wnd, "FnameBlock");
                    if (Fname == null || Fname.Replace(" ", "").Length == 0) SetRedBlockControll(wnd, "LoginBlock");
                    if (Fname == null || Fname.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "PassBlock");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateSysUser(Fname, Login, Pass);
                        //UpdateAllDataView();

                        ShowMessageToUser(resultStr); // показать сообщение 
                        SetNullValuesToProperties(); // обнулить свойства
                        wnd.Close();
                    }
                });
            }
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