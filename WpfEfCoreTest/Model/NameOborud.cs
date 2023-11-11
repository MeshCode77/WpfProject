#nullable disable

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class NameOborud : INotifyPropertyChanged
    {
        private int _faktSrokExpl;

        private int _srokExpl;
        private int _stoimost1Ed;
        private int economicEffect;
        private int id;
        private int kolEdNameOb;


        private string nameOborud1;

        public int Id { get; set; }
        public string NameOborud1 { get; set; }

        [NotMapped]
        public int KolEdNameOb
        {
            get => kolEdNameOb;
            set
            {
                kolEdNameOb = value;
                OnPropertyChanged(nameof(KolEdNameOb));
            }
        }


        [NotMapped]
        public int EconomicEffect
        {
            get => economicEffect;
            set
            {
                economicEffect = value;
                OnPropertyChanged(nameof(EconomicEffect));
            }
        }

        [NotMapped]
        public int Stoimost1Ed //{ get; set; }
        {
            get => _stoimost1Ed;
            set
            {
                _stoimost1Ed = value;
                OnPropertyChanged(nameof(Stoimost1Ed));
            }
        }

        [NotMapped]
        public int SrokExpl
        {
            get => _srokExpl;
            set
            {
                _srokExpl = value;
                OnPropertyChanged(nameof(SrokExpl));
            }
        }

        [NotMapped]
        public int FaktSrokExpl
        {
            get => _faktSrokExpl;
            set
            {
                _faktSrokExpl = value;
                OnPropertyChanged(nameof(FaktSrokExpl));
            }
        }


        public virtual ICollection<F111> F111s { get; set; }


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