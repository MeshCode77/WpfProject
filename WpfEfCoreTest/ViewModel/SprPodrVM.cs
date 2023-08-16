using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class SprPodrVM : INotifyPropertyChanged
    {
        private ObservableCollection<Podr> _filteredPodr;

        private string _filterPodr;

        // получить все подразделения

        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        // команда кнопки отмены 
        private RelayCommand cancelPodr;


        // Команда удаления подразделения
        private RelayCommand deletePodrCmd;


        private string editNewPodr;

        // Команда редактирования наименования подразделения
        private RelayCommand editPodr;


        private string newNamePodr;

        // команда открытия окна для редактирования наименования подразделения
        private RelayCommand openWndEditPodrNameCmd;

        private ObservableCollection<Podr> podrs;

        public SprPodrVM()
        {
            FilteredPodr = OnFilter();
        }

        public static string namePodr { get; set; }

        public string FilterPodr
        {
            get => _filterPodr;
            set
            {
                _filterPodr = value;
                OnPropertyChanged(nameof(FilteredPodr));
                DataTransfer.FilterPodr = FilterPodr;
                FilteredPodr = OnFilter();
            }
        }


        public ObservableCollection<Podr> FilteredPodr
        {
            get => _filteredPodr;
            set
            {
                _filteredPodr = value;
                OnPropertyChanged(nameof(FilteredPodr));
            }
        }

        public ObservableCollection<Podr> Podrs
        {
            get => podrs;
            set
            {
                podrs = value;
                OnPropertyChanged();
            }
        }

        // свойство выбранного подразделения
        public static Podr SelectedPodr { get; set; }

        public string NewNamePodr
        {
            get => newNamePodr;
            set
            {
                newNamePodr = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Podr> AllPodrs
        {
            get => allPodrs;
            set
            {
                allPodrs = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenWndEditPodrNameCmd
        {
            get
            {
                return openWndEditPodrNameCmd ?? new RelayCommand(obj =>
                {
                    var resultStr = "Ничего не выбрано";

                    if (SelectedPodr != null)
                    {
                        //OpenWndEditPodrNameMethod(SelectedPodr);
                        var editPodrWnd = new EditPodrWnd(SelectedPodr);
                        editPodrWnd.ShowDialog();
                    }
                });
            }
        }

        public string EditNewPodr
        {
            get => editNewPodr;
            set
            {
                editNewPodr = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand EditPodr
        {
            get
            {
                return editPodr ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var result = "Ошибка";

                    if (namePodr == null || namePodr.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "NewNamePodrBlock");
                    }
                    else
                    {
                        result = DataWorker.EditPodr(SelectedPodr, namePodr);
                        UpdatePodrView();

                        SetNullValuesToProperties();
                        ShowMessageToUser(result);
                        wnd.Close();
                    }
                });
            }
        }

        public RelayCommand DeletePodrCmd
        {
            get
            {
                return deletePodrCmd ?? new RelayCommand(obj =>
                    {
                        var resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedPodr != null) resultStr = DataWorker.DeletePodr(SelectedPodr);

                        //обновление
                        UpdatePodrView();
                        SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                    }
                );
            }
        }

        public RelayCommand CancelPodr
        {
            get
            {
                return cancelPodr ?? new RelayCommand(obj =>
                    {
                        var wnd = obj as Window;

                        SetNullValuesToProperties();


                        //MainWindow mw = new MainWindow();
                        //mw.lvPodr.ItemsSource = AllPodrs;

                        wnd.Close();
                    }
                );
            }
        }

        private ObservableCollection<Podr> OnFilter()
        {
            if (string.IsNullOrEmpty(DataTransfer.FilterPodr))
            {
                AllPodrs = DataWorker.GetAllPodrs();
                return new ObservableCollection<Podr>(AllPodrs); // create a new collection
            }

            AllPodrs = DataWorker.GetAllPodrs();
            FilteredPodr = AllPodrs.Where(i => i.NamePodr.Contains(DataTransfer.FilterPodr)).ToObservableCollection();
            return FilteredPodr;
        }

        private void OpenWndEditPodrNameMethod(Podr selectedPodr)
        {
            var editPodrWnd = new EditPodrWnd(selectedPodr);
            editPodrWnd.ShowDialog();
        }


        private void SetNullValuesToProperties()
        {
            NewNamePodr = null;
        }

        private void ShowMessageToUser(string result)
        {
            var messageView = new MessageView(result);
            messageView.ShowDialog();
            //SetCenterPositionAndOpen(messageView);
        }

        private void SetRedBlockControll(Window wnd, string blockName)
        {
            var block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        private void UpdatePodrView()
        {
            AllPodrs = DataWorker.GetAllPodrs();
            SprPodrWindow.UpdatePodrsView.ItemsSource = null;
            SprPodrWindow.UpdatePodrsView.Items.Clear();
            SprPodrWindow.UpdatePodrsView.ItemsSource = AllPodrs;
            SprPodrWindow.UpdatePodrsView.Items.Refresh();
        }

        #region // Добавление подразделений

        // Добавление подразделений
        private RelayCommand addPodr;

        public RelayCommand AddPodr
        {
            get
            {
                return addPodr ?? new RelayCommand(obj =>
                {
                    var wnd = obj as Window;

                    var result = "";

                    if (NewNamePodr == null || NewNamePodr.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(wnd, "NamePodrBlock");
                    }
                    else
                    {
                        result = DataWorker.CreatePodr(NewNamePodr);
                        UpdatePodrView();

                        ShowMessageToUser(result);
                        SetNullValuesToProperties();
                        //wnd.Close();
                    }
                });
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