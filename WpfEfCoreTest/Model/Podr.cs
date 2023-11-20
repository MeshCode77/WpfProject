using MathCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfEfCoreTest.Model
{
    public partial class Podr : INotifyPropertyChanged
    {
        private int id;
        private string namePodr;
        public Podr()
        {
            //Users = new HashSet<User>();
        }

        //public int Id { get; set; }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        //public string NamePodr { get; set; }

        public string NamePodr
        {
            get { return namePodr; }
            set
            {
                namePodr = value;
                OnPropertyChanged("NamePodr");
            }
        }

        public virtual ICollection<User> Users { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
