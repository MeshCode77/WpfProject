using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.ViewModel
{
    internal class UserSysVM : INotifyPropertyChanged
    {
        // получить все подразделения
        private ObservableCollection<UserSy> allUserSys = DataWorker.GetAllUserSys();
        private UserSy selectedUserSys;

        public UserSysVM()
        {
            AllUserSys = DataWorker.GetAllUserSys();
        }

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