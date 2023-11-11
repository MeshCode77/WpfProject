using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.Model
{
    public class User : INotifyPropertyChanged
    {
        private string fname;
        private int id;
        private int idPodr;

        private Podr idPodrNavigation;

        //private User idUserNavigation;
        private string lname;
        private string mname;


        [NotMapped] // атрибут указывающий что свойство не будет  участвствовать в схеме EF БД 
        public Podr UserPodr => DataWorker.GetPodrById(idPodr);


        [NotMapped] // атрибут указывающий что свойство не будет  участвствовать в схеме EF БД 
        public User PodrUsers => DataWorker.GetUsersById(idPodr);


        [NotMapped] // атрибут указывающий что свойство не будет  участвствовать в схеме EF БД 
        public Info UserInfo => DataWorker.GetInfoById(id);

        [NotMapped] // атрибут указывающий что свойство не будет  участвствовать в схеме EF БД 
        public F111 UserF111 => DataWorker.GetF111ById(id);


        //public int Id { get; set; }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }


        public int IdPodr
        {
            get => idPodr;
            set
            {
                idPodr = value;
                OnPropertyChanged();
            }
        }


        public string Lname
        {
            get => lname;
            set
            {
                lname = value;
                OnPropertyChanged();
            }
        }

        public string Fname
        {
            get => fname;
            set
            {
                fname = value;
                OnPropertyChanged();
            }
        }

        public string Mname
        {
            get => mname;
            set
            {
                mname = value;
                OnPropertyChanged();
            }
        }


        public virtual Podr IdPodrNavigation
        {
            get => idPodrNavigation;
            set
            {
                idPodrNavigation = value;
                OnPropertyChanged();
            }
        }


        //public virtual Podr IdPodrNavigation { get; set; }
        public virtual ObservableCollection<F111> F111s { get; set; }
        public virtual ObservableCollection<Info> Infos { get; set; }

        //public virtual Info IdInfoNavigation { get; set; }
        //public virtual ICollection<UserSy> UserSies { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}