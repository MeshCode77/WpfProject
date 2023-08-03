#nullable disable

using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class Info : INotifyPropertyChanged
    {
        private string doljnost;
        private User idUserNavigation;
        private string nameComp;

        public int Id { get; set; }

        public int IdUser { get; set; }

        //public string NameComp { get; set; }
        public string NameComp
        {
            get => nameComp;
            set
            {
                nameComp = value;
                OnPropertyChanged();
            }
        }

        public string Login { get; set; }
        public string Pass { get; set; }
        public string Ip { get; set; }

        public string Mac { get; set; }
        //public string Doljnost { get; set; }

        public string Doljnost
        {
            get => doljnost;
            set
            {
                doljnost = value;
                OnPropertyChanged();
            }
        }


        public string Vtel { get; set; }
        public virtual User IdUserNavigation { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}