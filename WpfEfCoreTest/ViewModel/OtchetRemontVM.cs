using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.ViewModel
{
    public class OtchetRemontVM : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        public ObservableCollection<OtchetRemont> allOtchetRem = DataWorker.GetAllOtchetRemont();

        // команда очистки таблицы OtchetRemont
        private RelayCommand clearOtchetRemont;

        // команда сохранения файла
        private RelayCommand saveCommand;

        public OtchetRemontVM()
        {
        }

        public OtchetRemontVM(IDialogService dialogService, IFileService fileService)
        {
            _dialogService = dialogService;
            _fileService = fileService;
        }


        public ObservableCollection<OtchetRemont> AllOtchetRem
        {
            get => allOtchetRem;
            set
            {
                allOtchetRem = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               if (_dialogService.SaveFileDialog())
                               {
                                   _fileService.Save(_dialogService.FilePath, AllOtchetRem.ToList());
                                   _dialogService.ShowMessage("Файл сохранен");
                               }
                           }
                           catch (Exception ex)
                           {
                               _dialogService.ShowMessage(ex.Message);
                           }
                       }));
            }
        }

        public RelayCommand ClearOtchetRemont
        {
            get
            {
                return clearOtchetRemont ??= new RelayCommand(obj =>
                {
                    var res = DataWorker.ClearOtchetRemont();

                    AllOtchetRem.Clear();
                    DataWorker.GetAllOtchetRemont();

                    foreach (var column in MainWindowVM.f111s) // нашли столбец Remont и установили ему значение true
                        column.Remont = false;

                    //MainWindowVM.f111s = new ObservableCollection<F111>();
                    //MainWindowVM.f111s = DataWorker.GetAllDataF111();

                    MessageBox.Show(res);
                });
            }
        }


        public void AddToRemont(User fio, F111 selected, string title)
        {
            var result = "косяк 111";
            try
            {
                var otchRem = new OtchetRemont
                {
                    Idf111 = selected.Id,
                    User = fio.Lname + " " + fio.Fname + " " + fio.Mname,
                    Podr = selected.Podr,
                    InvNum = selected.InvNum,
                    NumForm = selected.NumForm,
                    NameOborud = selected.Model,
                    BeginDate = selected.GtDate =
                        DateTime.Now, // в бд выбран тип Date а надо было DateTime тогда было бы и время 
                    EndDate = selected.OutDate,
                    ZavodNum = selected.ZavodNum,
                    Title = title
                };

                AllOtchetRem.Add(otchRem);

                result = DataWorker.AddToRemontDB(otchRem, selected.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            MessageBox.Show(result);
        }

        public void AddToRemontCompleted(User fio, F111 selected, string titleCompleted)
        {
            var result = "косяк 111";
            try
            {
                var otchRem = new OtchetRemont
                {
                    Idf111 = selected.Id,
                    User = fio.Lname + " " + fio.Fname + " " + fio.Mname,
                    Podr = selected.Podr,
                    InvNum = selected.InvNum,
                    NumForm = selected.NumForm,
                    NameOborud = selected.Model,
                    BeginDate =
                        null, // = DateTime.Now, // в бд выбран тип Date а надо было DateTime тогда было бы и время 
                    EndDate = selected.OutDate = DateTime.Now,
                    ZavodNum = selected.ZavodNum,
                    Title = null,
                    TitleComplected = titleCompleted
                };

                //AllOtchetRem.Add(otchRem);

                result = DataWorker.AddToRemontDB(otchRem, selected.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            MessageBox.Show(result);
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