#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class Formular : INotifyPropertyChanged
    {
        public Formular()
        {
            Dgms = new HashSet<Dgm>();
            Spisanies = new HashSet<Spisanie>();
        }

        public int Id { get; set; }
        public int Idf111 { get; set; }
        public int IdKomplect { get; set; }
        public string NameKomplect { get; set; }
        public string NumForm { get; set; }
        public string InvNum { get; set; }
        public string Model { get; set; }
        public string Count { get; set; }
        public string Serial { get; set; }
        public DateTime DataTo { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public string NumAkt { get; set; }
        public string YearProd { get; set; }
        public string GarantyTo { get; set; }

        public virtual Komplect IdKomplectNavigation { get; set; }
        public virtual F111 Idf111Navigation { get; set; }
        public virtual ICollection<Dgm> Dgms { get; set; }
        public virtual ICollection<Spisanie> Spisanies { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}