using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public static TestContext db;


        private static ObservableCollection<F111> allDataF111; // = GetAllDataF111(); 

        private ObservableCollection<NameOborud> _allUnionColl;

        //свойста для отображения коллекции значений колличества едениц техники для всей коллекции NameOborud
        private ObservableCollection<int> _countOborudColl = DataWorker.GetOborudColl();

        private ObservableCollection<int> _kolEdNameOb; // = DataWorker.GetOborudColl();


        private ObservableCollection<NameOborud> _kolNameOb;

        private ObservableCollection<NameOborud> _nameUnionColl; // = DataWorker.GetAllNameOborud();


        private string _newItemText;
        private string _newItemTextFaktSrokExpl;
        private string _newItemTextSrokExpl;

        //выбор строки оборужования
        private NameOborud _selectedNameOborud;

        //получить все Оборудование
        private ObservableCollection<NameOborud> allOborud = DataWorker.GetAllNameOborud();

        //получить все ПЭВМ 
        private ObservableCollection<F111> allPvm = DataWorker.AllPvm();

        //колличество оборудования
        private int countOborud;

        // выбраное оборудование
        private NameOborud selectedNameOborudCB;

        // объединенная коллекция 
        private CompositeCollection unionCollection; // { get; set; }

        //объединенная коллекция
        //private ObservableCollection<NameOborud> unionColl;// = UnionColl(); // UnionNameOborud();


        #region Конструктор класса

        public OtchetAllOborudVM()
        {
            AllUnionColl = UnionNameOborud();

            //allDataF111 = DataWorker.GetAllDataF111(); // для обновления данных на форме при изменении колличества оборудования      

            //CountOborudColl = DataWorker.GetOborudColl();

            AddToStoim1EdCollCmd = new RelayCommand(AddItem, CanAddItem);

            AddToSrokExplCmd = new RelayCommand(AddItemSrokExpl, CanAddItemSrokExpl);

            AddToFaktSrokExplCmd = new RelayCommand(AddItemFaktSrokExpl, CanAddItemFaktSrokExpl);

            //SummaEconomEffect();

            //EditToStoim1EdCollCmd = new RelayCommand(Update);
        }

        #endregion

        public static int SummaAllEconomEffect { get; set; }

        // свойства типа коллекции для заполнения 
        public static ObservableCollection<int> Stoim1EdColl { get; set; } = new();

        public static ObservableCollection<int> SrokExpColl { get; set; } = new();

        public static ObservableCollection<int> FaktSrokExpColl { get; set; } = new();

        public static ObservableCollection<int> EconomicEffectColl { get; set; } = new();


        //public ObservableCollection<int> KolEdNameOb
        //{
        //    get => _kolEdNameOb;
        //    set
        //    {
        //        _kolEdNameOb = value;
        //        OnPropertyChanged(nameof(KolEdNameOb));
        //    }
        //}


        //public CollectionViewSource NameUnionColl
        //{
        //    get => _nameUnionColl;
        //    set
        //    {
        //        _nameUnionColl = value;
        //        OnPropertyChanged(nameof(NameUnionColl));
        //    }
        //}


        // коллекция наименований NameOborud
        public ObservableCollection<NameOborud> NameOborudColl { get; set; }

        // коллекция колличества оборудования int
        //public ObservableCollection<int> KolEdNameOb { get; set; } 

        // коллекция стоимости за 1 еденицу техники
        public ObservableCollection<int> Stoim1EObColl { get; set; }

        public CompositeCollection UnionCollection
        {
            get => unionCollection;
            set
            {
                unionCollection = value;
                OnPropertyChanged(nameof(UnionCollection));
            }
        }

        public ObservableCollection<NameOborud> AllUnionColl
        {
            get => _allUnionColl;
            set
            {
                _allUnionColl = value;
                OnPropertyChanged(nameof(AllUnionColl));
            }
        }


        public ObservableCollection<F111> AllPvm
        {
            get => allPvm;
            set
            {
                allPvm = value;
                OnPropertyChanged(nameof(AllPvm));
            }
        }

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

        // свойство для колличества едениц техники
        public ObservableCollection<int> CountOborudColl
        {
            get => _countOborudColl;
            set
            {
                _countOborudColl = value;
                OnPropertyChanged(nameof(CountOborudColl));
            }
        }


        // свойство для отображения колличества для одной записи из NameOborud 
        public int CountOborud
        {
            get => countOborud;

            set
            {
                countOborud = value;
                OnPropertyChanged(nameof(CountOborud));
            }
        }


        public NameOborud SelectedNameOborudCB
        {
            get => selectedNameOborudCB;
            set
            {
                selectedNameOborudCB = value;
                OnPropertyChanged(nameof(SelectedNameOborudCB));

                CountOborud = 0;

                if (allDataF111 != null)
                    foreach (var temp in allDataF111)
                        if (temp.IdnameOborud == SelectedNameOborudCB.Id)
                            CountOborud++;
            }
        }

        //public ObservableCollection<int> Stoim1EdColl
        //{
        //    get => _stoim1EdColl;
        //    set
        //    {
        //        if (_stoim1EdColl != value)
        //        {
        //            _stoim1EdColl = value;
        //            OnPropertyChanged(nameof(Stoim1EdColl));
        //        }
        //    }
        //}

        public RelayCommand ResultCmd
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    //Effect = (int.Parse(Stoimost1Ed) / int.Parse(SrokExpl) *
                    //          (int.Parse(SrokExpl) - int.Parse(FactSrokExpl)) * int.Parse(KolEd)).ToString();

                    //Effect = Stoimost1Ed / SrokExpl * (SrokExpl - FactSrokExpl) * KolEd;

                    //EconomicEffect = Stoimost1Ed / SrokExpl * (SrokExpl - FactSrokExpl) * KolEd;

                    //UnionNameOborud();
                });
            }
        }


        private int _selectedSt1Ed { get; set; }

        public int SelectedSt1Ed
        {
            get => _selectedSt1Ed;
            set
            {
                _selectedSt1Ed = value;
                OnPropertyChanged(nameof(SelectedSt1Ed));
            }
        }

        //public static int SummaAllEconomEffect
        //{
        //    get => _summaEconomEffect;
        //    set
        //    {
        //        _summaEconomEffect = value;
        //        OnPropertyChanged(nameof(SummaAllEconomEffect));
        //    }
        //}


        public ObservableCollection<int> GetAllOborud()
        {
            var count = 0;
            var id = 0;

            for (var i = 0; i < AllOborud.Count; i++)
            {
                id = AllOborud[i].Id;

                for (var j = 0; j < allDataF111.Count; j++)
                    if (allDataF111[j].IdnameOborud == id)
                        count++;

                CountOborud = count;
                if (CountOborudColl != null) CountOborudColl.Add(count);
            }

            return CountOborudColl;
        }

        private int CountOb()
        {
            var count = 0;
            var id = 0;

            for (var i = 0; i < AllOborud.Count; i++)
            {
                id = AllOborud[i].Id;

                for (var j = 0; j < allDataF111.Count; j++)
                    if (allDataF111[j].IdnameOborud == id)
                        count++;

                CountOborud = count;
                //CountOborudColl.Add(count);
            }

            return count;
        }


        // метод объединения 2 коллекции с помощью классов CollectionViewSource и CompositeCollection
        public CompositeCollection UnionColl()
        {
            NameOborudColl = DataWorker.GetAllNameOborud();

            //KolEdNameOb = DataWorker.GetOborudColl();

            //Stoim1EObColl = Stoim1EdColl;  // коллекцию заполняем в ручную

            //var source1 = new CollectionViewSource();
            //source1.Source = AllOborud;

            //var source2 = new CollectionViewSource();
            //source2.Source = CountOborudColl;

            //var source3 = new CollectionViewSource();
            //source3.Source = Stoim1EdColl;

            //Объединяем коллекции в составную коллекцию:
            UnionCollection = new CompositeCollection
            {
                new CollectionContainer { Collection = NameOborudColl },
                //new CollectionContainer { Collection = KolEdNameOb },
                new CollectionContainer { Collection = Stoim1EObColl }
            };

            return UnionCollection; // объединенная коллекция
        }


        ////// метод для заполенния данными дополнительного поля KolEdNameOb значениями из коллекции CountColl
        public static ObservableCollection<NameOborud> UnionNameOborud()
        {
            // коллекция для NameOborud
            var unionColl = new ObservableCollection<NameOborud>();
            unionColl = DataWorker.GetAllNameOborud();

            // коллекция для CountColl
            //var valColl = new ObservableCollection<int>();
            //valColl = DataWorker.GetOborudColl();        

            var stoim1EdColl = DataWorker.GetOborudColl();

            var temp1 = 0;

            foreach (var temp in unionColl)
            {
                temp.KolEdNameOb = stoim1EdColl[temp1];

                if (Stoim1EdColl.Count == 0)
                    //MessageBox.Show("Заполните стоимость 1 еденицы", "Внимание", MessageBoxButton.OK);
                    return null;

                foreach (var item in Stoim1EdColl)
                {
                    temp.Stoimost1Ed = Stoim1EdColl[temp1];
                    temp.SrokExpl = SrokExpColl[temp1];
                    temp.FaktSrokExpl = FaktSrokExpColl[temp1];

                    temp.EconomicEffect = temp.Stoimost1Ed / temp.SrokExpl * (temp.FaktSrokExpl - temp.SrokExpl) *
                                          temp.KolEdNameOb;

                    EconomicEffectColl.Add(temp.EconomicEffect);
                }

                SummaAllEconomEffect = temp.EconomicEffect + SummaAllEconomEffect;

                temp1++;
            }

            return unionColl;
        }


        //public static int SummaEconomEffect()
        //{
        //    var SummaAllEconomEffect = 0;

        //    foreach (var item in EconomicEffectColl)
        //        SummaAllEconomEffect = item + SummaAllEconomEffect;

        //    return SummaAllEconomEffect;
        //}

        #region Свойство команд для добавления и обновления коллекций

        public RelayCommand AddToStoim1EdCollCmd { get; }

        public RelayCommand AddToSrokExplCmd { get; }

        public RelayCommand AddToFaktSrokExplCmd { get; }

        public RelayCommand UpdateToStoim1EdCollCmd { get; }
        public RelayCommand UpdateToSrokExpllCmd { get; }
        public RelayCommand UpdateToFactSrokExplCmd { get; }

        #endregion

        #region Свойства для коллекций используемых для расчета Экономического эффекта

        public string NewItemText
        {
            get => _newItemText;
            set
            {
                _newItemText = value;
                OnPropertyChanged(nameof(NewItemText));

                // вызываем команду AddItemCommand при каждом изменении свойства NewItemText
                //AddToStoim1EdCollCmd.RaiseCanExecuteChanged();
            }
        }

        public string NewItemTextSrokExpl
        {
            get => _newItemTextSrokExpl;
            set
            {
                _newItemTextSrokExpl = value;
                OnPropertyChanged(nameof(NewItemTextSrokExpl));
            }
        }


        public string NewItemTextFaktSrokExpl
        {
            get => _newItemTextFaktSrokExpl;
            set
            {
                _newItemTextFaktSrokExpl = value;
                OnPropertyChanged(nameof(NewItemTextFaktSrokExpl));
            }
        }

        #endregion

        #region методы для команд

        private void AddItem(object obj)
        {
            if (AllOborud.Count != Stoim1EdColl.Count)
            {
                Stoim1EdColl.Add(Convert.ToInt32(NewItemText));
                NewItemText = "";
            }
            else
            {
                NewItemText = "Все данные заполнены!!!";
            }
        }

        //Stpim1Ed
        private bool CanAddItem(object arg)
        {
            // разрешаем выполнение команды только если свойство NewItemText не пустое
            return !string.IsNullOrEmpty(NewItemText);
        }


        //Srok Expl
        private void AddItemSrokExpl(object obj)
        {
            if (AllOborud.Count != SrokExpColl.Count)
            {
                SrokExpColl.Add(Convert.ToInt32(NewItemTextSrokExpl));
                NewItemTextSrokExpl = "";
            }
            else
            {
                NewItemTextSrokExpl = "Все данные заполнены!!!";
            }
        }

        private bool CanAddItemSrokExpl(object arg)
        {
            // разрешаем выполнение команды только если свойство NewItemText не пустое
            return !string.IsNullOrEmpty(NewItemTextSrokExpl);
        }


        //FactExpl
        private void AddItemFaktSrokExpl(object obj)
        {
            if (AllOborud.Count != FaktSrokExpColl.Count)
            {
                FaktSrokExpColl.Add(Convert.ToInt32(NewItemTextFaktSrokExpl));
                NewItemTextFaktSrokExpl = "";
            }
            else
            {
                NewItemTextFaktSrokExpl = "Все данные заполнены!!!";
            }
        }

        private bool CanAddItemFaktSrokExpl(object arg)
        {
            // разрешаем выполнение команды только если свойство NewItemText не пустое
            return !string.IsNullOrEmpty(NewItemTextFaktSrokExpl);
        }


        private void Update(object obj)
        {
            if (AllOborud.Count != Stoim1EdColl.Count) NewItemText = "";
        }

        #endregion


        #region // Cвойства для расчета экономического эфекта

        ////cвойства для расчета экономического эфекта
        //private static string _stoimost1Ed; // стоимость за новую еденицу при покупке
        //private static string _srokExpl; // срок службы
        //private static string _factSrokExpl; // фактический срок службы
        //private static string _kolEd; // колличество еденич
        //private static string _effect; // экономический эфект

        //cвойства для расчета экономического эфекта
        public static int _stoimost1Ed; // стоимость за новую еденицу при покупке
        public static int _srokExpl; // срок службы
        public static int _faktSrokExpl; // фактический срок службы
        public static int _kolEd; // колличество еденич
        public static int _effect;
        private int economicEffect; // экономический эфект

        public int Stoimost1Ed
        {
            get => _stoimost1Ed;
            set
            {
                _stoimost1Ed = value;
                OnPropertyChanged(nameof(Stoimost1Ed));
            }
        }


        public int SrokExpl
        {
            get => _srokExpl;
            set
            {
                _srokExpl = value;
                OnPropertyChanged(nameof(SrokExpl));
            }
        }

        public int FaktSrokExpl
        {
            get => _faktSrokExpl;
            set
            {
                _faktSrokExpl = value;
                OnPropertyChanged(nameof(FaktSrokExpl));
            }
        }


        public int KolEd
        {
            get => _kolEd;
            set
            {
                _kolEd = value;
                OnPropertyChanged(nameof(KolEd));
            }
        }


        public int Effect
        {
            get => _effect;
            set
            {
                _effect = value;
                OnPropertyChanged(nameof(Effect));
            }
        }

        public int EconomicEffect
        {
            get => economicEffect;

            set
            {
                economicEffect = value;
                OnPropertyChanged(nameof(EconomicEffect));
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