using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public static string namePodr { get; set; }

        private ObservableCollection<Podr> podrs;
        public ObservableCollection<Podr> Podrs
        {
            get { return podrs; }
            set
            {
                podrs = value;
                OnPropertyChanged("Podrs");
            }
        }

        // свойство выбранного подразделения
        public static Podr SelectedPodr { get; set; }


        private string newNamePodr;
        public string NewNamePodr
        {
            get { return newNamePodr; }
            set
            {
                newNamePodr = value;
                OnPropertyChanged("NewNamePodr");
            }
        }

        // получить все подразделения

        private ObservableCollection<Podr> allPodrs = DataWorker.GetAllPodrs();

        public ObservableCollection<Podr> AllPodrs
        {
            get { return allPodrs; }
            set
            {
                allPodrs = value;
                OnPropertyChanged("AllPodrs");
            }
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
                    Window wnd = obj as Window;

                    string result = "";

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


        // команда открытия окна для редактирования наименования подразделения
        private RelayCommand openWndEditPodrNameCmd;

        public RelayCommand OpenWndEditPodrNameCmd
        {
            get
            {
                return openWndEditPodrNameCmd ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";

                    if (SelectedPodr != null)
                    {
                        //OpenWndEditPodrNameMethod(SelectedPodr);
                        EditPodrWnd editPodrWnd = new EditPodrWnd(SelectedPodr);
                        editPodrWnd.ShowDialog();
                    }
                });
            }
        }

        private void OpenWndEditPodrNameMethod(Podr selectedPodr)
        {
            EditPodrWnd editPodrWnd = new EditPodrWnd(selectedPodr);
            editPodrWnd.ShowDialog();
        }


        private  string editNewPodr;
        public string EditNewPodr
        {
            get { return editNewPodr; }
            set
            {
                editNewPodr = value;
                OnPropertyChanged("EditNewPodr");
            }
        }

        // Команда редактирования наименования подразделения
        private RelayCommand editPodr;
        public RelayCommand EditPodr
        {
            get
            {
                return editPodr ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;

                    string result = "Ошибка";

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


        // Команда удаления подразделения
        private RelayCommand deletePodrCmd;
        public RelayCommand DeletePodrCmd
        {
            get
            {
                return deletePodrCmd ?? new RelayCommand(obj =>
                    {
                        string resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedPodr != null)
                        {
                            resultStr = DataWorker.DeletePodr(SelectedPodr);
                           
                        }

                        //обновление
                        UpdatePodrView();
                        SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                    }
                );
            }
        }

        // команда кнопки отмены 
        private RelayCommand cancelPodr;
        public RelayCommand CancelPodr
        {
            get
            {
                return cancelPodr ?? new RelayCommand(obj =>
                    {
                        Window wnd = obj as Window;

                        SetNullValuesToProperties();
                       

                        //MainWindow mw = new MainWindow();
                        //mw.lvPodr.ItemsSource = AllPodrs;
                       
                        wnd.Close();
                    }
                );
            }
        }



        private void SetNullValuesToProperties()
        {
            NewNamePodr = null;
        }

        private void ShowMessageToUser(string result)
        {
            MessageView messageView = new MessageView(result);
            messageView.ShowDialog();
            //SetCenterPositionAndOpen(messageView);
        }

        private void SetRedBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
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
