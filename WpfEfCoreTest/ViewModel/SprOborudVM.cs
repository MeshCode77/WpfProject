using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;
using WpfEfCoreTest.View;

namespace WpfEfCoreTest.ViewModel
{
    public class SprOborudVM : INotifyPropertyChanged
    {

        #region Реализация интерфейса INotyfyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public static string nameOborud1 { get; set; }


        //private ObservableCollection<NameOborud> nameOborud;
        //public ObservableCollection<NameOborud> NameOborud
        //{
        //    get { return nameOborud; }
        //    set
        //    {
        //        nameOborud = value;
        //        OnPropertyChanged(nameof(NameOborud));
        //    }
        //}

        //свойство выбранного оборудования;
        public  static NameOborud SelectedNO { get; set; }


        // получить все подразделения

        private ObservableCollection<NameOborud> allNameOborud = DataWorker.GetAllNameOborud();
        public ObservableCollection<NameOborud> AllNameOborud
        {
            get { return allNameOborud; }
            set
            {
                allNameOborud = value;
                OnPropertyChanged("AllNameOborud");
            }
        }

        // Вывод сообщения
        private void ShowMessageToUser(string result)
        {
            MessageView messageView = new MessageView(result);
            messageView.ShowDialog();
            //SetCenterPositionAndOpen(messageView);
        }

        // Обновление данных в LvSprNO
        private void UpdateNOView()
        {
            AllNameOborud = DataWorker.GetAllNameOborud();
            SprOborudView.UpdateNO.ItemsSource = null;
            SprOborudView.UpdateNO.Items.Clear();
            SprOborudView.UpdateNO.ItemsSource = AllNameOborud;
            SprOborudView.UpdateNO.Items.Refresh();
        }

        // свойство для нового оборудования
        private string newNameOb;
        public string NewNameOb
        {
            get { return newNameOb; }
            set
            {
                newNameOb = value;
                OnPropertyChanged("NewNameOb");
            }
        }


        #region команда кнопки отмены 

        // команда кнопки отмены 
        private RelayCommand cancelNameOborud;
        public RelayCommand CancelNameOborud
        {
            get
            {
                return cancelNameOborud ?? new RelayCommand(obj =>
                    {
                        Window wnd = obj as Window;

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
        private RelayCommand deleteNO;
        public RelayCommand DeleteNO
        {
            get
            {
                return deleteNO ?? new RelayCommand(obj =>
                    {
                        string resultStr = "Ничего не выбрано";
                        //если сотрудник
                        if (SelectedNO != null)
                        {
                            resultStr = DataWorker.DeleteNO(SelectedNO);

                        }

                        //обновление
                        UpdateNOView();
                        //SetNullValuesToProperties();
                        ShowMessageToUser(resultStr);
                    }
                );
            }
        }

        #endregion

        #region Команда открытия окна для редактирования наименования оборудования

        // команда открытия окна для редактирования наименования оборудования
        private RelayCommand editNO;
        public RelayCommand EditNOWnd
        {
            get
            {
                return editNO ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";

                    if (SelectedNO != null)
                    {
                        //OpenWndEditPodrNameMethod(SelectedPodr);
                        EditNOWnd editNOWnd = new EditNOWnd(SelectedNO);
                        editNOWnd.ShowDialog();
                    }
                });
            }
        }

        #endregion

        #region Команда редактирования наименования оборудования

        // Команда редактирования наименования оборудования
        private RelayCommand editNOCmd;
        public RelayCommand EditNOCmd
        {
            get
            {
                return editNOCmd ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;

                    string result = "Ошибка";

                    if (nameOborud1 == null || nameOborud1.Replace(" ", "").Length == 0)
                    {
                        //SetRedBlockControll(wnd, "NewNamePodrBlock");
                        MessageBox.Show("Ошибка ввода данных", "Проверте вводимые данные");
                    }
                    else
                    {
                        result = DataWorker.EditNO(SelectedNO, nameOborud1);
                        UpdateNOView();

                        //SetNullValuesToProperties();
                        ShowMessageToUser(result);
                        wnd.Close();
                    }
                });
            }
        }

        #endregion

        #region Добавление оборудования

        // Добавление оборудования
        private RelayCommand addOborud;
        public RelayCommand AddOborud
        {
            get
            {
                return addOborud ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;

                    string result = "";

                    if (NewNameOb == null || NewNameOb.Replace(" ", "").Length == 0)
                    {
                        //SetRedBlockControll(wnd, "NamePodrBlock");
                        MessageBox.Show("Error!!!", "Проверте вводимые данные");
                    }
                    else
                    {
                        result = DataWorker.CreateNameOb(NewNameOb);
                        UpdateNOView();

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
