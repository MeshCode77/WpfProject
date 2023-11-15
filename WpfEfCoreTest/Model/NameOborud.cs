#nullable disable

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.Model
{
    public class NameOborud : INotifyPropertyChanged
    {
        //public ObservableCollection<int> EconomEfectColl = new();
        public static int economicEffect;
        private int _allSummaEconomEfect;
        private int _faktSrokExpl;
        private int _srokExpl;
        private int _stoimost1Ed;
        private int id;
        private int kolEdNameOb;
        private string nameOborud1;


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
        public int EconomicEffect //{ get; set; }
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
        public int SrokExpl //{ get; set; }
        {
            get => _srokExpl;
            set
            {
                _srokExpl = value;
                OnPropertyChanged(nameof(SrokExpl));
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

                EconomicEffect = Stoimost1Ed / SrokExpl * (FaktSrokExpl - SrokExpl) * KolEdNameOb;

                OtchetAllOborudVM.GetColl(EconomicEffect);

                //OtchetAllOborudVM.SummaAllEconomEffect = OtchetAllOborudVM.collEf.Sum();
            }
        }

        [NotMapped]
        public int AllSummaEconomEffect //{ get; set; }
        {
            get => _allSummaEconomEfect;
            set
            {
                _allSummaEconomEfect = value;
                OnPropertyChanged(nameof(AllSummaEconomEffect));
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