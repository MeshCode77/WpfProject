

#nullable disable

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public partial class Komplect : INotifyPropertyChanged
    {
        public Komplect()
        {
            Formulars = new HashSet<Formular>();
            SprDgms = new HashSet<SprDgm>();
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }


        private string nameKompl;
        public string NameKompl
        {
            get => nameKompl;
            set
            {
                nameKompl = value;
                OnPropertyChanged(nameof(NameKompl));
            }
        }

        

        public virtual ICollection<Formular> Formulars { get; set; }
        public virtual ICollection<SprDgm> SprDgms { get; set; }



        #region Реализация интерфейса INotyfyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
