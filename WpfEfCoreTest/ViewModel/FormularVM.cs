using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class FormularVM : INotifyPropertyChanged
    {
        // получить все данные из Formular по id F111
        private static ObservableCollection<Formular> allDataFormular =
            DataWorker.GetAllDataFormularToIdF111(DataTransfer.IdF111);

        private static Formular _selectedRowFormular;

        private readonly Formular addForm;

        private ObservableCollection<Formular> _formular;

        private ObservableCollection<Formular> allDataFormularToIdF111;


        // получить все данные таблицы Komplect
        private ObservableCollection<Komplect> allKomplects = DataWorker.GetAllKomplect();


        private bool buttonOnOff;
        public TestContext db;

        private RelayCommand deleteFormularCmd;


        // команда открытия окна для добавления данных в формуляр
        public RelayCommand openFormularCmd;


        private Komplect selectedNameKomplCB;

        #region Конструктор класса

        public FormularVM()
        {
            //this.addForm = addForm;
            db = new TestContext();

            db.Formulars.Load();
            db.Formulars.ToObservableCollection();

            db.F111s.Load();
            db.F111s.ToObservableCollection();

            db.Komplects.Load();
            db.Komplects.ToObservableCollection();
            AllKomplects = db.Komplects.Local.ToObservableCollection();

            // для отображения в comboBox значения
            //if (SelectedRowFormular != null)
            //{
            //    var resNameOborud = db.Komplects.FirstOrDefault(x => x.Id == SelectedRowFormular.IdKomplect);
            //    //var resGtDate = db.F111s.FirstOrDefault(x => x.GtDate == SelectedRowF111.GtDate);


            //    if (resNameOborud != null)
            //        SelectedNameKomplCB = resNameOborud;
            //    //GtDate = resGtDate.GtDate;
            //}
        }

        #endregion

        public static Formular selectedRowFormular { get; set; }

        public Formular SelectedRowFormular
        {
            get => selectedRowFormular;
            set
            {
                selectedRowFormular = value;
                OnPropertyChanged(nameof(SelectedRowFormular));
                ButtonOnOff = true;
            }
        }

        public ObservableCollection<Formular> AllDataFormular
        {
            get
            {
                if (allDataFormular != null) return allDataFormular;

                MessageBox.Show("Данные отсутствуют");
                return null;
            }
            set
            {
                allDataFormular = value;
                OnPropertyChanged(nameof(AllDataFormular));
            }
        }

        public bool ButtonOnOff
        {
            get => buttonOnOff;
            set
            {
                buttonOnOff = value;
                OnPropertyChanged(nameof(ButtonOnOff));
            }
        }

        public Komplect SelectedNameKomplCB
        {
            get => selectedNameKomplCB;
            set
            {
                selectedNameKomplCB = value;
                OnPropertyChanged(nameof(SelectedNameKomplCB));
            }
        }


        private ObservableCollection<Formular> formulars { get; set; }

        public ObservableCollection<Formular> FilteredFormular
        {
            get => MainWindowVM.formular;
            set
            {
                MainWindowVM.formular = value;
                OnPropertyChanged();

                MainWindowVM.formular.Clear();

                if (MainWindowVM._selectedF111 != null)
                    db.Formulars.Where(u => u.Idf111 == MainWindowVM._selectedF111.Id).ToObservableCollection()
                        .Foreach(x => MainWindowVM.formular.Add(x));
            }
        }

        public ObservableCollection<Komplect> AllKomplects
        {
            get => allKomplects;
            set
            {
                allKomplects = value;
                OnPropertyChanged(nameof(AllKomplects));
            }
        }

        public RelayCommand OpenFormularCmd
        {
            get
            {
                return openFormularCmd ?? new RelayCommand(odj =>
                {
                    var openFormular = new AddFormularView();
                    openFormular.ShowDialog();
                });
            }
        }

        public ObservableCollection<Formular> AllDataFormularToIdF111
        {
            get
            {
                if (allDataFormularToIdF111 != null) return allDataFormularToIdF111;

                MessageBox.Show("Данные отсутствуют");
                return null;
            }
            set
            {
                allDataFormularToIdF111 = value;
                OnPropertyChanged(nameof(AllDataFormularToIdF111));
            }
        }

        public RelayCommand DeleteFormularCmd
        {
            get
            {
                return deleteFormularCmd ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedRowFormular != null)
                        {
                            resultStr = DataWorker.DeleteFormular(SelectedRowFormular);

                            ShowMessageToUser(resultStr);

                            UpdateFormular();

                            //обнуление свойств
                            SetNullValuesToProperties();
                        }
                    }
                );
            }
        }

        // собственный метод вывода сообщения
        private void ShowMessageToUser(string message)
        {
            var messageView = new MessageView(message);
            messageView.ShowDialog();
        }

        private void SetNullValuesToProperties()
        {
            SelectedKomplect = null;
            NumForm = null;
            InvNum = null;
            Model = null;
            Count = null;
            Serial = null;
            //DataTo = null;
            DateIn = DateTime.Now;
            //DateOut = null;
            NumAkt = null;
            YearProd = null;
            GarantyTo = null;
            //NameKomplect = null;
        }


        // метод контроля ввода данных в textBox ы
        private void SetRedBlockTextBox(Window wnd, string blockName)
        {
            var block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }


        #region Обновлени данных в контроле ListView LvFormular

        public void UpdateFormular()
        {
            var result = "Данные не изменены";

            var mwvm = new MainWindowVM();
            mwvm.UpdateFormular();

            AllDataFormular = DataWorker.GetAllDataFormularToIdF111(DataTransfer.IdF111);

            FormularView.AllDataFormularToIdF111.ItemsSource = null;
            FormularView.AllDataFormularToIdF111.Items.Clear();
            FormularView.AllDataFormularToIdF111.ItemsSource = AllDataFormular;
            FormularView.AllDataFormularToIdF111.Items.Refresh();
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

        #region Свойства таблицы Formular

        public static int Id { get; set; }
        public static int Idf111 { get; set; }

        public static int IdKomplect { get; set; }

        //public static string NameKompl { get; set; }
        public static string NumForm { get; set; }
        public static string InvNum { get; set; }
        public static string Model { get; set; }
        public static string Count { get; set; }
        public static string Serial { get; set; }
        public static DateTime DataTo { get; set; }
        public static DateTime DateIn { get; set; }
        public static DateTime? DateOut { get; set; }
        public static string NumAkt { get; set; }
        public static string YearProd { get; set; }
        public static string GarantyTo { get; set; }
        public static Komplect SelectedKomplect { get; set; }

        #endregion

        #region Команда добавления  данных в формуляр

        // Команда добавления новых данных в формуляр
        private RelayCommand addFormularCmd;

        public RelayCommand AddFormularCmd
        {
            get
            {
                return addFormularCmd ?? new RelayCommand(obj =>
                {
                    //var addForm = new Formular();

                    var form = new Formular
                    {
                        NumForm = NumForm,
                        InvNum = InvNum,
                        Model = Model,
                        Count = Count,
                        Serial = Serial,
                        DataTo = DataTo,
                        DateIn = DateIn,
                        DateOut = DateOut,
                        NumAkt = NumAkt,
                        YearProd = YearProd,
                        GarantyTo = GarantyTo,
                        NameKomplect = SelectedKomplect.NameKompl
                    };

                    var wnd = obj as Window;
                    var result = "";

                    if (string.IsNullOrEmpty(NumForm) || NumForm.Length == 0)
                    {
                        MessageBox.Show("Ввведите номер формуляра", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }


                    if (string.IsNullOrEmpty(InvNum) || InvNum.Length == 0)
                    {
                        MessageBox.Show("Ввведите Инвентарный номер", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }


                    if (string.IsNullOrEmpty(Model) || Model.Length == 0)
                    {
                        MessageBox.Show("Ввведите Модель", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }


                    if (string.IsNullOrEmpty(Count) || Count.Length == 0)
                    {
                        MessageBox.Show("Ввведите колличество", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }


                    if (string.IsNullOrEmpty(Serial) || Serial.Length == 0)
                    {
                        MessageBox.Show("Ввведите Заводской/Серийный номер", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }


                    if (string.IsNullOrEmpty(DateIn.ToString()) || DateIn == DateTime.MinValue)
                    {
                        DateIn = DateTime.Now;
                        MessageBox.Show("Ввведите Дату ввода", "Сообщение", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    if (SelectedKomplect == null)
                    {
                        MessageBox.Show("Не выбрано комплектующее!");
                    }
                    else
                    {
                        //result = DataWorker.AddDataFormular(DataTransfer.IdF111, SelectedKomplect.Id, NumForm, InvNum,
                        //    Serial, Model,
                        //    Count, DataTo, DateIn, DateOut, NumAkt, YearProd, GarantyTo, SelectedKomplect.NameKompl);

                        result = DataWorker.AddDataFormular(DataTransfer.IdF111, SelectedKomplect.Id, form,
                            SelectedKomplect.NameKompl);

                        UpdateFormular();

                        ShowMessageToUser(result);

                        SetNullValuesToProperties();

                        wnd.Close();
                    }
                });
            }
        }

        #endregion


        #region команда открытия окна для редактирования Formulara

        public RelayCommand openEditFormularCmd;

        public RelayCommand OpenEditFormularCmd
        {
            get
            {
                return openEditFormularCmd ?? new RelayCommand(odj =>
                {
                    var result = "Ничего не выбрано";

                    if (SelectedRowFormular != null)
                    {
                        var editFormular = new EditFormularView(SelectedRowFormular);

                        editFormular.ShowDialog();
                    }
                });
            }
        }

        #endregion

        #region Команда редактирования Formulara

        private RelayCommand editFormularCmd;

        public RelayCommand EditFormularCmd
        {
            get
            {
                return editFormularCmd ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        var result = "Не выбраны данные";

                        if (SelectedRowFormular != null)
                        {
                            result = DataWorker.EditFormular(SelectedRowFormular, SelectedNameKomplCB.Id,
                                SelectedNameKomplCB.NameKompl, NumForm, InvNum, Model, Count, Serial, DataTo, DateIn,
                                DateOut, NumAkt, YearProd, GarantyTo);

                            UpdateFormular();

                            ShowMessageToUser(result);

                            wnd.Close();

                            SetNullValuesToProperties();
                        }
                        else
                        {
                            ShowMessageToUser(result);
                        }
                    }
                );
            }
        }

        #endregion
    }
}