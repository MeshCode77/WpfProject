using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    internal class UserSysVM : INotifyPropertyChanged
    {
        // получить все подразделения
        private ObservableCollection<UserSy> allUserSys; // = DataWorker.GetAllUserSys();
        private UserSy selectedUserSys;

        public RelayCommand sigIn;

        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Fname { get; set; }
        public string Role { get; set; }
        public int? IdUser { get; set; }


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