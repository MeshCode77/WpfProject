

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.Model
{
    public partial class Info :  INotifyPropertyChanged
    {
        private User idUserNavigation;
        private string doljnost;
        private string nameComp;

        public int Id { get; set; }
        public int IdUser { get; set; }
        //public string NameComp { get; set; }
        public string NameComp
        {
            get { return nameComp; }
            set
            {
                nameComp = value;
                OnPropertyChanged("NameComp");
            }
        }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        //public string Doljnost { get; set; }

        public string Doljnost
        {
            get { return doljnost; }
            set
            {
                doljnost = value;
                OnPropertyChanged("Doljnost");
            }
        }


        public string Vtel { get; set; }
        public virtual User IdUserNavigation { get; set; }

        //public virtual User IdUserNavigation
        //{
        //    get { return idUserNavigation; }
        //    set
        //    {
        //        idUserNavigation = value;
        //        OnPropertyChanged("IdUserNavigation");
        //    }
        //}

        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
