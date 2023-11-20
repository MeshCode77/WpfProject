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
        public double _economicEffect;
        private int _faktSrokExpl;
        private int _srokExpl;
        private int _stoimost1Ed;
        private int kolEdNameOb;

        public int Id { get; set; }

        public string NameOborud1 { get; set; }

        [NotMapped]
        public int KolEdNameOb //{ get; set; }
        {
            get => kolEdNameOb;
            set
            {
                kolEdNameOb = value;
                OnPropertyChanged(nameof(KolEdNameOb));
            }
        }


        [NotMapped]
        public double EconomicEffect //{ get; set; }
        {
            get => _economicEffect;
            set
            {
                _economicEffect = value;
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

                var res = SrokExpl * (FaktSrokExpl - SrokExpl) * KolEdNameOb;

                if (res == 0) return;

                EconomicEffect = Stoimost1Ed / (double)res;
            }
        }

        [NotMapped]
        public int SrokExpl //{ get; set; }
        {
            get => _srokExpl;
            set
            {
                _srokExpl = value;
                OnPropertyChanged(nameof(SrokExpl));

                var res = SrokExpl * (FaktSrokExpl - SrokExpl) * KolEdNameOb;

                if (res == 0) return;

                EconomicEffect = Stoimost1Ed / (double)res;
            }
        }

        [NotMapped]
        public int FaktSrokExpl //{ get; set; }
        {
            get => _faktSrokExpl;
            set
            {
                _faktSrokExpl = value;
                OnPropertyChanged(nameof(FaktSrokExpl));

                // Проверка на 0

                var res = SrokExpl * (FaktSrokExpl - SrokExpl) * KolEdNameOb;

                if (res == 0) return;

                EconomicEffect = Stoimost1Ed / (double)res;

                //OtchetAllOborudVM.GetColl(EconomicEffect);

                //OtchetAllOborudVM.SummaAllEconomEffect = OtchetAllOborudVM.collEf.Sum();
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