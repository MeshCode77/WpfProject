using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.ViewModel
{
    public class OtchetAllOborudVM : INotifyPropertyChanged
    {
        //выбор строки оборужования
        private NameOborud _selectedNameOborud;

        //получить все Оборудование
        private ObservableCollection<NameOborud> allOborud;

        // Данные для ComboBox - все наименования
        public ObservableCollection<NameOborud> AllOborud
        {
            get => allOborud;
            set
            {
                allOborud = value;
                OnPropertyChanged(nameof(AllOborud));
            }
        }

        public NameOborud SelectedOborud
        {
            get => _selectedNameOborud;
            set
            {
                _selectedNameOborud = value;
                OnPropertyChanged(nameof(SelectedOborud));

                //CountOb();
            }
        }

        public RelayCommand ResultCmd
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    AllSummaEconomEffect = AllOborud.Sum(o => o.EconomicEffect);
                });
            }
        }

        // метод объединения данных таблицы NameOborud и другой коллекции (о колличестве оборуд.) в одном ListView
        public ObservableCollection<NameOborud> UnionCollNo()
        {
            var tempColl = new ObservableCollection<NameOborud>();
            tempColl = DataWorker.GetAllNameOborud(); // коллекция данных из таблицы NameOborud

            var countColl = new ObservableCollection<int>();
            countColl = DataWorker
                .GetOborudColl(); // коллекция данных из F111 о колличестве оборудования по каждой поззиции

            var temp1 = 0;

            foreach (var temp in tempColl)
            {
                if (countColl.Count == 0)
                    return null;
                // заполняет добавленный столбец в ListView который не учавствует в схеме БД таблицы NameOborud другой коллекцией
                temp.KolEdNameOb = countColl[temp1];
                temp1++;
            }


            return tempColl;
        }


        // метод объединения 2 коллекции с помощью классов CollectionViewSource и CompositeCollection
        //public CompositeCollection UnionColl()
        //{
        //    NameOborudColl = DataWorker.GetAllNameOborud();

        //    //KolEdNameOb = DataWorker.GetOborudColl();

        //    //Stoim1EObColl = Stoim1EdColl;  // коллекцию заполняем в ручную

        //    //var source1 = new CollectionViewSource();
        //    //source1.Source = AllOborud;

        //    //var source2 = new CollectionViewSource();
        //    //source2.Source = CountOborudColl;

        //    //var source3 = new CollectionViewSource();
        //    //source3.Source = Stoim1EdColl;

        //    //Объединяем коллекции в составную коллекцию:
        //    UnionCollection = new CompositeCollection
        //    {
        //        new CollectionContainer { Collection = NameOborudColl },
        //        //new CollectionContainer { Collection = KolEdNameOb },
        //        new CollectionContainer { Collection = Stoim1EObColl }
        //    };

        //    return UnionCollection; // объединенная коллекция
        //}


        ////// метод для заполнения данными дополнительного поля KolEdNameOb значениями из коллекции CountColl
        //public ObservableCollection<NameOborud> UnionNameOborud()
        //{
        //    // коллекция для NameOborud
        //    var unionColl = new ObservableCollection<NameOborud>();
        //    unionColl = DataWorker.GetAllNameOborud();

        //    var stoim1EdColl = DataWorker.GetOborudColl(); // количество оборудования по категориям

        //    var temp1 = 0;

        //    foreach (var temp in unionColl)
        //    {
        //        temp.KolEdNameOb = stoim1EdColl[temp1];

        //        if (Stoim1EdColl.Count == 0)
        //            //MessageBox.Show("Заполните стоимость 1 еденицы", "Внимание", MessageBoxButton.OK);
        //            return null;

        //        foreach (var item in Stoim1EdColl)
        //        {
        //            temp.Stoimost1Ed = Stoim1EdColl[temp1];
        //            temp.SrokExpl = SrokExpColl[temp1];
        //            temp.FaktSrokExpl = FaktSrokExpColl[temp1];

        //            temp.EconomicEffect = temp.Stoimost1Ed / temp.SrokExpl * (temp.FaktSrokExpl - temp.SrokExpl) *
        //                                  temp.KolEdNameOb;

        //            EconomicEffectColl.Add(temp.EconomicEffect);
        //        }

        //        SummaAllEconomEffect = temp.EconomicEffect + SummaAllEconomEffect;

        //        temp1++;
        //    }

        //    return unionColl;
        //}


        #region Конструктор класса

        public OtchetAllOborudVM()
        {
            AllOborud = UnionCollNo();
        }

        public OtchetAllOborudVM(int sum)
        {
            AllSummaEconomEffect = sum;
        }

        #endregion

        #region Cвойства для расчета экономического эфекта

        private double _allSummaEconomEfect;

        public double AllSummaEconomEffect
        {
            get => _allSummaEconomEfect;
            set
            {
                _allSummaEconomEfect = value;
                OnPropertyChanged(nameof(AllSummaEconomEffect));
            }
        }

        #endregion


        #region Реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}