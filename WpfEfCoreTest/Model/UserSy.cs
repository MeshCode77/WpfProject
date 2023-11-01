#nullable disable

using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class UserSy : INotifyPropertyChanged
    {
        private int id { get; set; }
        private string fname { get; set; }
        private string login { get; set; }
        private string pass { get; set; }


        //public virtual Role IdRoleNavigation { get; set; }
        //public virtual User IdUserNavigation { get; set; }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Fname
        {
            get => fname;
            set
            {
                fname = value;
                OnPropertyChanged(nameof(Fname));
            }
        }

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Pass
        {
            get => pass;
            set
            {
                pass = value;
                OnPropertyChanged(nameof(Pass));
            }
        }

        /// <summary>
        ///     /////////////////////////////////////////////////////////////////
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}