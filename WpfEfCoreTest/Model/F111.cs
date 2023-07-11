#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class F111 : INotifyCollectionChanged
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdnameOborud { get; set; }
        public string Podr { get; set; }
        public string Model { get; set; }
        public string KartNum { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public string ZavodNum { get; set; }
        public DateTime GtDate { get; set; } = DateTime.Now;
        public DateTime? OutDate { get; set; }
        public bool Remont { get; set; }
        public bool Spisan { get; set; }


        public virtual User IdUserNavigation { get; set; }

        public virtual NameOborud IdnameOborudNavigation { get; set; }

        public virtual ObservableCollection<Formular> Formulars { get; set; }

        public virtual ICollection<OtchetRemont> OtchetRemonts { get; set; }

        //public virtual ICollection<Remont> Remonts { get; set; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}