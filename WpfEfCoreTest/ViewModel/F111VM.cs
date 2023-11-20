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
    public class F111VM : INotifyPropertyChanged
    {
        // получить все данные из F111 по id пользователя
        private static ObservableCollection<F111> allDataF111 = GetAllDataF111();
        private readonly TestContext db;

        // получить все подразделения
        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        private bool buttonOnOff;


        private NameOborud selectedNameOborudCB;


        // выбранное подразделение в comboBox при добавлении в карточку F111
        private Podr selectedPodr;


        #region Конструктор класса

        //private int idUser = MainWindowVM._selectedUser.Id;

        // Конструктор класса
        public F111VM()
        {
            db = new TestContext();

            db.F111s.Load();

            db.NameOboruds.Load();
            AllNameOborud = db.NameOboruds.Local.ToObservableCollection();

            //this.idUser = idUser;


            // для отображения в comboBox значения
            if (SelectedRowF111 != null)
            {
                var resNameOborud = db.NameOboruds.FirstOrDefault(x => x.Id == SelectedRowF111.IdnameOborud);
                var resGtDate = db.F111s.FirstOrDefault(x => x.GtDate == SelectedRowF111.GtDate);


                if (resNameOborud != null && resGtDate != null)
                {
                    SelectedNameOborudCB = resNameOborud;
                    GtDate = resGtDate.GtDate;
                }
            }


            KartNumMethod();
        }

        #endregion

        public ObservableCollection<Podr> AllPodrs
        {
            get => allPodrs;
            set
            {
                allPodrs = value;
                OnPropertyChanged();
            }
        }


        public static F111 selectedRowF111 { get; set; }

        public F111 SelectedRowF111
        {
            get => selectedRowF111;
            set
            {
                selectedRowF111 = value;
                OnPropertyChanged(nameof(SelectedRowF111));
                ButtonOnOff = true;
            }
        }

        public bool ButtonOnOff
        {
            get => buttonOnOff;
            set
            {
                buttonOnOff = value;
                OnPropertyChanged(nameof(ButtonOnOff));
                //if (FilteredF111s == null)
                //    ButtonOnOff = true;
            }
        }


        private static ObservableCollection<F111> f111s { get; set; }


        public ObservableCollection<F111> AllDataF111
        {
            get
            {
                if (allDataF111 != null) return allDataF111;

                MessageBox.Show("Данные отсутствуют");
                return null;
            }
            set
            {
                allDataF111 = value;
                OnPropertyChanged(nameof(AllDataF111));
            }
        }

        public NameOborud SelectedNameOborudCB
        {
            get => selectedNameOborudCB;
            set
            {
                selectedNameOborudCB = value;
                OnPropertyChanged(nameof(SelectedNameOborudCB));
            }
        }

        public Podr SelectedPodr
        {
            get => selectedPodr;
            set
            {
                selectedPodr = value;
                OnPropertyChanged(nameof(SelectedPodr));
            }
        }


        public ObservableCollection<F111> FilteredF111s
        {
            get => MainWindowVM.f111s;
            set
            {
                MainWindowVM.f111s = value;
                OnPropertyChanged(nameof(FilteredF111s));

                f111s.Clear();
                if (FilteredF111s == null)
                    ButtonOnOff = true;
                if (MainWindowVM._selectedUser != null)
                    db.F111s.Where(u => u.IdUser == MainWindowVM._selectedUser.Id).ToObservableCollection()
                        .Foreach(x => MainWindowVM.f111s.Add(x));
            }
        }

        // метод получения данные из F111 по id пользователя
        public static ObservableCollection<F111> GetAllDataF111()
        {
            if (allDataF111 != null)
            {
                allDataF111 = DataWorker.GetAllDataF111(MainWindowVM._selectedUser.UserF111.IdUser);
                return allDataF111;
            }

            return null;
        }

        #region Метод для проверки и генерации номера карточки Ф.111

        // Метод для проверки и генерации номера карточки Ф.111
        private void KartNumMethod()
        {
            if (FilteredF111s.Count == 0)
            {
                var rnd = new Random(); // экземпляр класса Random
                var temp = rnd.Next(1, 1000).ToString(); // колличество элементов от 1 до 1000

                var chechIsExist = db.F111s.Any(el => el.KartNum == temp); // 

                if (!chechIsExist)
                {
                    KartNum = temp;
                    return;
                }

                MessageBox.Show("Номер катрочки уже существует");
            }
        }

        #endregion

        #region метод обнуления свойств F111

        private void SetNullValuesToProperties()
        {
            Podr = null;
            Model = null;
            KartNum = null;
            NumForm = null;
            InvNum = null;
            ZavodNum = null;
            GtDate = DateTime.Now;
            OutData = null;
        }

        #endregion

        #region метод обновления карточки ф111

        // команда обновления карточки ф111
        public void UpdateF111()
        {
            var result = "Данные не обновлены";

            if (DataTransfer.IdUser != 0)
            {
                var mwvm = new MainWindowVM();
                mwvm.UpdateF111ToUser();


                AllDataF111 = DataWorker.GetAllDataF111(DataTransfer.IdUser);

                F111View.AllDataF111ToUser.ItemsSource = null;
                F111View.AllDataF111ToUser.Items.Clear();
                F111View.AllDataF111ToUser.ItemsSource = AllDataF111;
                F111View.AllDataF111ToUser.Items.Refresh();
            }
        }

        #endregion

        private void ShowMessageToUser(string message)
        {
            var messageView = new MessageView(message);
            messageView.ShowDialog();
        }

        // метод валидации ввода данных в textBox ы
        private void SetRedBlockTextBox(Window wnd, string blockName)
        {
            var block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        #region свойства для таблицы F111

        //свойства для F111
        public static string Podr { get; set; }
        public static string Model { get; set; }
        public static string KartNum { get; set; }
        public static string NumForm { get; set; }
        public static string InvNum { get; set; }
        public static string ZavodNum { get; set; }
        public static DateTime GtDate { get; set; } = DateTime.Now;
        public static DateTime? OutData { get; set; }
        public static NameOborud SelectedNameOborud { get; set; }

        public static bool? Remont { get; set; }
        public static bool? Spisan { get; set; }

        #endregion


        #region получить все данные таблицы NameOborud

        // получить все данные таблицы NameOborud
        private ObservableCollection<NameOborud> allNameOborud = DataWorker.GetAllNameOborud();

        public ObservableCollection<NameOborud> AllNameOborud
        {
            get => allNameOborud;
            set
            {
                allNameOborud = value;
                OnPropertyChanged(nameof(AllNameOborud));
            }
        }

        #endregion

        #region Коммады открытия окон для добавления и редактирования

        // команда открытия окна для добавления F111
        public RelayCommand openAddF111Wnd;

        public RelayCommand OpenAddF111Wnd
        {
            get
            {
                return openAddF111Wnd ?? new RelayCommand(odj =>
                {
                    SetNullValuesToProperties();
                    var addF111 = new AddF111View();
                    addF111.ShowDialog();
                });
            }
        }

        // команда открытия окна для редактирования F111
        public RelayCommand openEditF111Win;

        public RelayCommand OpenEditF111Win
        {
            get
            {
                return openEditF111Win ?? new RelayCommand(odj =>
                {
                    var result = "Ничего не выбрано";

                    if (SelectedRowF111 != null)
                    {
                        var editF111 = new EditF111View(SelectedRowF111);
                        editF111.ShowDialog();
                    }
                });
            }
        }

        #endregion

        #region Команда добавления новых данных в карточку Ф.111

        // Команда добавления новых данных в карточку Ф.111
        private RelayCommand addF111Cmd;

        public RelayCommand AddF111Cmd
        {
            get
            {
                return addF111Cmd ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;
                    var result = "";
                    if (string.IsNullOrEmpty(Model) || Podr.Length == 0) SetRedBlockTextBox(wnd, "PodrBlock");
                    if (string.IsNullOrEmpty(Model) || Model.Length == 0) SetRedBlockTextBox(wnd, "ModelBlock");
                    if (string.IsNullOrEmpty(KartNum) || KartNum.Length == 0) SetRedBlockTextBox(wnd, "KartNumBlock");
                    if (string.IsNullOrEmpty(NumForm) || NumForm.Length == 0) SetRedBlockTextBox(wnd, "NumFormBlock");
                    if (string.IsNullOrEmpty(InvNum) || InvNum.Length == 0) SetRedBlockTextBox(wnd, "InvNumBlock");
                    if (string.IsNullOrEmpty(ZavodNum) || ZavodNum.Length == 0)
                        SetRedBlockTextBox(wnd, "ZavodNumBlock");

                    if (SelectedNameOborud == null)
                    {
                        MessageBox.Show("Не выбрано оборудование!");
                    }
                    else
                    {
                        if (DataTransfer.IdUser != 0)
                        {
                            result = DataWorker.AddDataF111(DataTransfer.IdUser, SelectedNameOborud.Id,
                                SelectedPodr.NamePodr,
                                SelectedNameOborud.NameOborud1, KartNum, NumForm, InvNum, ZavodNum, GtDate, OutData);

                            UpdateF111();

                            ShowMessageToUser(result);

                            SetNullValuesToProperties();

                            wnd.Close();
                        }
                    }
                });
            }
        }

        #endregion

        #region команда редактирования F111

        // команда редактирования F111

        private RelayCommand editF111Command;

        public RelayCommand EditF111Command
        {
            get
            {
                return editF111Command ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        var resultStr = "Не выбраны данные";
                        //string noPodr = "Не выбрано подразделение";


                        if (SelectedRowF111 != null)
                        {
                            resultStr = DataWorker.EditF111(SelectedRowF111, SelectedNameOborudCB.Id,
                                SelectedNameOborudCB.NameOborud1, KartNum, NumForm, InvNum, ZavodNum, GtDate, OutData);

                            UpdateF111();

                            ShowMessageToUser(resultStr);

                            wnd.Close();

                            SetNullValuesToProperties();
                        }
                        else
                        {
                            ShowMessageToUser(resultStr);
                            SetRedBlockTextBox(wnd, "LnameBlock");
                        }
                    }
                );
            }
        }

        #endregion

        #region команда удаления

        // команда удаления 

        private RelayCommand deleteF111Command;

        public RelayCommand DeleteF111Command
        {
            get
            {
                return deleteF111Command ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedRowF111 != null)
                        {
                            resultStr = DataWorker.DeleteF111(SelectedRowF111);

                            ShowMessageToUser(resultStr);

                            UpdateF111();

                            //обнуление свойств
                            SetNullValuesToProperties();
                        }
                    }
                );
            }
        }

        #endregion

        #region команда кнопки отмены

        // команда кнопки отмены 

        private RelayCommand cancelF111;
        private object _selectedUser;

        public RelayCommand CancelF111
        {
            get
            {
                return cancelF111 ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        //SetNullValuesToProperties();
                        wnd.Close();
                    }
                );
            }
        }

        #endregion


        #region реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}