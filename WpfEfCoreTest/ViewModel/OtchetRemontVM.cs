using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;
using WpfEfCoreTest.Model.Data;

namespace WpfEfCoreTest.ViewModel
{
    public class OtchetRemontVM : INotifyPropertyChanged
    {
        public ObservableCollection<OtchetRemont> allOtchetRem = DataWorker.GetAllOtchetRemont();


        public ObservableCollection<OtchetRemont> AllOtchetRem
        {
            get => allOtchetRem;
            set
            {
                allOtchetRem = value;
                OnPropertyChanged();
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
                    BeginDate = selected
                        .GtDate, // = DateTime.Now, // в бд выбран тип Date а надо было DateTime тогда было бы и время 
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

        //public void RemoveToRemont(
        //        int id) //не удаление объекта из таблицы OtchetRemont а снятие флажка в колонке Remont таблицы F111
        //{
        //    var result = DataWorker.RemoveToRemont(id);

        //    MessageBox.Show(result);
        //}

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