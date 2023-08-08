using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class SprKomplVM : INotifyPropertyChanged
    {
        private string _filterNK;
        // получить все подразделения

        private ObservableCollection<Komplect> allNameKompl = DataWorker.GetAllNameKomplects();

        // свойство для нового комплектующего
        private string newNameKompl;


        public static string nameKompl { get; set; }

        public string FilterNK
        {
            get => _filterNK;
            set
            {
                _filterNK = value;
                OnPropertyChanged(nameof(FilteredNK));
            }
        }


        public ObservableCollection<Komplect> FilteredNK
        {
            get
            {
                if (string.IsNullOrEmpty(FilterNK)) return allNameKompl;
                return new ObservableCollection<Komplect>(allNameKompl.Where(x => x.NameKompl == FilterNK));
            }
        }


        //свойство выбранного оборудования;
        public static Komplect SelectedNK { get; set; }

        public ObservableCollection<Komplect> AllNameKompl
        {
            get => allNameKompl;
            set
            {
                allNameKompl = value;
                OnPropertyChanged(nameof(AllNameKompl));
            }
        }

        public string NewNameKompl
        {
            get => newNameKompl;
            set
            {
                newNameKompl = value;
                OnPropertyChanged(nameof(NewNameKompl));
            }
        }

        // метод вывода сообщения
        private void ShowMessageToUser(string result)
        {
            var messageView = new MessageView(result);
            messageView.ShowDialog();
            //SetCenterPositionAndOpen(messageView);
        }

        // метод обновления данных в ListView LvSprNK 
        private void UpdateNKView()
        {
            AllNameKompl = DataWorker.GetAllNameKomplects();
            SprKomplView.UpdateNK.ItemsSource = null;
            SprKomplView.UpdateNK.Items.Clear();
            SprKomplView.UpdateNK.ItemsSource = AllNameKompl;
            SprKomplView.UpdateNK.Items.Refresh();
        }

        #region Реализация интерфейса INotyfyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region команда кнопки отмены

        // команда кнопки отмены 
        private RelayCommand cancelNameKompl;

        public RelayCommand CancelNameKompl
        {
            get
            {
                return cancelNameKompl ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        //SetNullValuesToProperties();


                        //MainWindow mw = new MainWindow();
                        //mw.lvPodr.ItemsSource = AllNameOborud;

                        wnd.Close();
                    }
                );
            }
        }

        #endregion

        #region Команда удаления оборудования

        // Команда удаления подразделения
        private RelayCommand deleteNK;

        public RelayCommand DeleteNK
        {
            get
            {
                return deleteNK ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedNK != null) resultStr = DataWorker.DeleteNK(SelectedNK);

                        //обновление
                        UpdateNKView();
                        //SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                    }
                );
            }
        }

        #endregion

        #region Команда открытия окна для редактирования наименования комплектующего

        // команда открытия окна для редактирования наименования комплектующего
        private RelayCommand editNK;

        public RelayCommand EditNKWnd
        {
            get
            {
                return editNK ?? new RelayCommand(obj =>
                {
                    var resultStr = "Ничего не выбрано";

                    if (SelectedNK != null)
                    {
                        //OpenWndEditPodrNameMethod(SelectedPodr);
                        var editNKWnd = new EditNKWnd(SelectedNK);
                        editNKWnd.ShowDialog();
                    }
                });
            }
        }

        #endregion

        #region Команда редактирования наименования комплектующего

        // Команда редактирования наименования комплектующего
        private RelayCommand editNKCmd;

        public RelayCommand EditNKCmd
        {
            get
            {
                return editNKCmd ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var result = "Ошибка";

                    if (nameKompl == null || nameKompl.Replace(" ", "").Length == 0)
                    {
                        //SetRedBlockControll(wnd, "NewNamePodrBlock");
                        MessageBox.Show("Проверте вводимые данные", "Ошибка ввода данных", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else
                    {
                        result = DataWorker.EditNK(SelectedNK, nameKompl);
                        UpdateNKView();

                        //SetNullValuesToProperties();
                        ShowMessageToUser(result);
                        wnd.Close();
                    }
                });
            }
        }

        #endregion

        #region Добавление комплектующего

        // Добавление комплектующего
        private RelayCommand addKompl;

        public RelayCommand AddKompl
        {
            get
            {
                return addKompl ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var result = "";

                    if (NewNameKompl == null || NewNameKompl.Replace(" ", "").Length == 0)
                    {
                        //SetRedBlockControll(wnd, "NamePodrBlock");
                        MessageBox.Show("Проверте вводимые данные!", "Ошибка!!!");
                    }
                    else
                    {
                        result = DataWorker.CreateNameKoml(NewNameKompl);
                        UpdateNKView();

                        ShowMessageToUser(result);
                        //SetNullValuesToProperties();
                        //wnd.Close();
                    }
                });
            }
        }

        #endregion
    }
}