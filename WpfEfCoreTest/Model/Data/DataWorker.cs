using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using WpfEfCoreTest.ViewModel;

namespace WpfEfCoreTest.Model.Data
{
    public static class DataWorker
    {
        public static F111 F1111 { get; private set; }

        // получить всех пользователей
        public static ObservableCollection<User> GetAllUsers()
        {
            using (var tc = new TestContext())
            {
                var result = tc.Users.ToObservableCollection();
                return result;
            }
        }

        // получить полльзователей по Id подразделения
        public static ObservableCollection<User> GetAllUsersToPodr(int id)
        {
            using (var tc = new TestContext())
            {
                //var podrId = lvPodr.SelectedItem as Podr;
                var result = tc.Users.Where(u => u.IdPodr == id);
                return result.ToObservableCollection();
            }
            //var podrId = tree.SelectedValue as Podr;
            //var users = tc.Users.Where(u => u.IdPodr == podrId.Id);
            //lvUsers.ItemsSource = users.ToList();
        }

        //получить все данные из F111
        public static ObservableCollection<F111> GetAllDataF111()
        {
            ObservableCollection<F111> result;

            using (var tc = new TestContext())
            {
                result = tc.F111s.ToObservableCollection();
            }

            if (result != null) return result;

            MessageBox.Show("Данные отсутствуют");
            return null;
        }


        public static ObservableCollection<F111> GetAllDataF111(int id)
        {
            using (var tc = new TestContext())
            {
                //var result = tc.F111s.Find(SelectedRowF111.Id);
                var result = tc.F111s.Where(u => u.IdUser == id).ToObservableCollection();

                if (result != null) return result;

                MessageBox.Show("Данные отсутствуют");
                return null;
            }
        }

        public static ObservableCollection<Komplect> GetAllNameKomplects()
        {
            using (var tc = new TestContext())
            {
                var result = tc.Komplects.ToObservableCollection();
                return result;
            }
        }


        // получить все подразделения
        public static ObservableCollection<Podr> GetAllPodrs()
        {
            using (var tc = new TestContext())
            {
                var result = tc.Podrs.ToObservableCollection();
                return result;
            }
        }

        public static ObservableCollection<Info> GetAllInfos()
        {
            using (var tc = new TestContext())
            {
                var result = tc.Infos.ToObservableCollection();
                return result;
            }
        }


        // создаем нового usera
        public static string CreateUser(string lname, string fname, string mname, string dolj, string nameComp,
            string login, string pass, string mac, string vtel,
            Podr IdPodrNavigation)
        {
            var result = "Уже существует";

            using (var tc = new TestContext())
            {
                try
                {
                    // проверяем наличие пользователя
                    var chechIsExist = tc.Users.Any(el =>
                        el.Lname == lname && el.Fname == fname && el.IdPodrNavigation == IdPodrNavigation);
                    var checkIsExistInfo = tc.Infos.Any(el => el.Doljnost == dolj);

                    if (!chechIsExist && !checkIsExistInfo)
                    {
                        var newUser = new User
                        {
                            Lname = lname,
                            Fname = fname,
                            Mname = mname,
                            IdPodr = IdPodrNavigation.Id // СВЯЗЬ FK(users) и PK(podrs)
                        };
                        tc.Users.Add(newUser);
                        tc.SaveChanges();


                        var newInfo = new Info
                        {
                            IdUser = newUser.Id,
                            Doljnost = dolj,
                            NameComp = nameComp,
                            Login = login,
                            Pass = pass,
                            Mac = mac,
                            Vtel = vtel
                        };

                        tc.Infos.Add(newInfo);
                        tc.SaveChanges();

                        result = "Сделано";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                return result;
            }
        }

        public static string DeleteNK(Komplect selectedNK)
        {
            var result = "Такого комплектующего не существует";

            using (var tc = new TestContext())
            {
                tc.Komplects.Remove(selectedNK);
                tc.SaveChanges();
                result = "Сделано! Комплектующее: " + selectedNK.NameKompl + " - удалено";
            }

            return result;
        }

        public static ObservableCollection<NameOborud> GetAllNameOborud()
        {
            using (var tc = new TestContext())
            {
                var result = tc.NameOboruds.ToObservableCollection();
                return result;
            }
        }


        // Получение Подразделения по Id Подразделения
        public static Podr GetPodrById(int idPodr)
        {
            using (var tc = new TestContext())
            {
                var podr = tc.Podrs.FirstOrDefault(p => p.Id == idPodr);
                return podr;
            }
        }

        public static string AddDataF111(int _selectedUser, int idNameOborud, string podr, string nameOborud,
            string kartNum,
            string numForm, string invNum, string zavodNum, DateTime gtDate, DateTime? outData)
        {
            var result = "Такое оборудование уже существует в карточке";

            try
            {
                using (var tc = new TestContext())
                {
                    //проверяем наличие вводимых данных в карточке пользователя
                    var checkIsExistF111 = tc.F111s.Any(el =>
                        el.InvNum == invNum && el.KartNum == kartNum && el.NumForm == numForm &&
                        el.ZavodNum == zavodNum);
                    if (!checkIsExistF111)
                    {
                        var newDataF111 = new F111
                        {
                            IdUser = _selectedUser, // Id пользователя для связи с табалицей Users
                            IdnameOborud = idNameOborud, // SelectedNameOborud для связи с таблицей nameOborud
                            Podr = podr,
                            Model =
                                nameOborud, // SelectedNameOborud.NameOborud1 т.к. в Model будет записываться значение имени оборудования типа string
                            KartNum = kartNum,
                            NumForm = numForm,
                            InvNum = invNum,
                            ZavodNum = zavodNum,
                            GtDate = gtDate,
                            OutDate = outData
                        };

                        tc.F111s.Add(newDataF111);
                        tc.SaveChanges();

                        result = "Данные успешно добавлены";
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //throw;
            }


            return result;
        }

        internal static string EditNK(Komplect selectedNK, string nameKompl)
        {
            var result = "такого комплектующего не существует";
            using (var db = new TestContext())
            {
                //check user is exist
                var nameK = db.Komplects.FirstOrDefault(p => p.Id == selectedNK.Id);

                if (nameK != null)
                {
                    nameK.NameKompl = nameKompl;

                    db.SaveChanges();
                    result = "Сделано! Нименование изменено на " + nameK.NameKompl;
                }
            }

            return result;
        }

        public static string CreateNameKoml(string newNameKompl)
        {
            var result = "Такое комплектующее уже имеется в БД";

            using (var tc = new TestContext())
            {
                var checkIsExist = tc.Komplects.Any(el => el.NameKompl == newNameKompl);
                if (!checkIsExist)
                {
                    var newNK = new Komplect
                    {
                        NameKompl = newNameKompl
                    };

                    tc.Komplects.Add(newNK);
                    tc.SaveChanges();

                    result = "Сделано";
                }

                return result;
            }
        }

        public static string EditNO(NameOborud selectedNO, string nameOborud1)
        {
            var result = "такого подразделения не существует";
            using (var db = new TestContext())
            {
                //check user is exist
                var nameOborud = db.NameOboruds.FirstOrDefault(p => p.Id == selectedNO.Id);

                if (nameOborud != null)
                {
                    nameOborud.NameOborud1 = nameOborud1;

                    db.SaveChanges();
                    result = "Сделано! Нименование изменено на " + nameOborud.NameOborud1;
                }
            }

            return result;
        }

        public static string CreateNameOb(string newNameOb)
        {
            var result = "Уже существует";

            using (var tc = new TestContext())
            {
                var checkIsExist = tc.NameOboruds.Any(el => el.NameOborud1 == newNameOb);
                if (!checkIsExist)
                {
                    var newNO = new NameOborud
                    {
                        NameOborud1 = newNameOb
                    };

                    tc.NameOboruds.Add(newNO);
                    tc.SaveChanges();

                    result = "Сделано";
                }

                return result;
            }
        }

        // Получение Подразделения по Id Подразделения
        public static User GetUsersById(int idPodr)
        {
            using (var tc = new TestContext())
            {
                var users = tc.Users.FirstOrDefault(u => u.Id == idPodr);
                return users;
            }
        }


        // Получение Info по IdUser Info
        public static Info GetInfoById(int id)
        {
            using (var tc = new TestContext())
            {
                var info = tc.Infos.FirstOrDefault(i => i.IdUser == id);
                return info;
            }
        }

        // Получение F111 по IdUser 
        public static F111 GetF111ById(int id)
        {
            using (var tc = new TestContext())
            {
                var f111 = tc.F111s.FirstOrDefault(i => i.IdUser == id);
                return f111;
            }
        }


        // редактирование данных пользователя
        public static string EditUser(User oldUser, string Lname, string Fname, string Mname, string dolj, string login,
            string pass, string mac, string vtel,
            string nameComp, Podr IdPodrNavigation)
        {
            var result = "Такого сотрудника не существует";
            using (var db = new TestContext())
            {
                //check user is exist
                var user = db.Users.FirstOrDefault(u => u.Id == oldUser.Id);
                var podr = db.Podrs.FirstOrDefault(p => p.Id == oldUser.IdPodr);
                var info = db.Infos.FirstOrDefault(i => i.IdUser == oldUser.Id);

                if (user != null)
                {
                    user.IdPodr = IdPodrNavigation.Id;
                    user.Lname = Lname;
                    user.Fname = Fname;
                    user.Mname = Mname;
                }

                // если объект info равен null т.е. нет данных в таблице Info для выбранного пользователя Info.IdUser <> User.Id  
                if (info == null)
                {
                    var newInfo = new Info() // создаем новые данные для выбранного пользователя
                    {
                        IdUser = user.Id,
                        Doljnost = dolj,
                        NameComp = nameComp,
                        Login = login,
                        Pass = pass,
                        Mac = mac,
                        Vtel = vtel
                    };

                    db.Infos.Add(newInfo); // и добавляем их
                }
                else
                {
                    info.IdUser = user.Id;
                    info.Doljnost = dolj;
                    info.NameComp = nameComp;
                    info.Login = login;
                    info.Pass = pass;
                    info.Mac = mac;
                    info.Vtel = vtel;
                }

                db.SaveChanges();
                result = "Сделано! Сотрудник " + user.Lname + " изменен";
            }

            return result;
        }

        // Редактирование карточки Ф.111
        public static string EditF111(F111 selectedRowF111, int idNameOborud, string nameOborud, string KartNum,
            string NumForm, string InvNum, string ZavodNum, DateTime GtDate, DateTime? OutData)
        {
            var result = "Данные не изменены!";
            using (var db = new TestContext())
            {
                //check user is exist
                var f111 = db.F111s.FirstOrDefault(u => u.Id == selectedRowF111.Id);

                if (f111 != null)
                {
                    //f111.Id = selectedRowF111.Id;
                    f111.IdnameOborud = idNameOborud; // SelectedNameOborud для связи с таблицей nameOborud
                    f111.Model =
                        nameOborud; // SelectedNameOborud.NameOborud1 т.к. в Model будет записываться значение имени оборудования типа string
                    f111.KartNum = KartNum;
                    f111.NumForm = NumForm;
                    f111.InvNum = InvNum;
                    f111.ZavodNum = ZavodNum;
                    f111.GtDate = GtDate;
                    f111.OutDate = OutData;

                    db.SaveChanges();
                    result = "Сделано! данные успешно  изменены!";
                    return result;
                }

                return result;
            }
        }

        public static string UpdateF111ToUser(int IdUser)
        {
            var result = "Данные не изменены";

            using (var tc = new TestContext())
            {
                //var result = tc.F111s.Where(u => u.IdUser == idUser).ToObservableCollection();
                //var result = tc.F111s.Update(F111VM.SelectedRowF111);

                var result1 = tc.F111s.FirstOrDefault(x => x.Id == IdUser);

                var tempF111 = new F111
                {
                    KartNum = F111VM.KartNum,
                    NumForm = F111VM.NumForm,
                    InvNum = F111VM.InvNum,
                    ZavodNum = F111VM.ZavodNum,
                    GtDate = F111VM.GtDate,
                    OutDate = F111VM.OutData
                };

                tc.SaveChanges();
                result = "Сделано! Данные успешно изменены";

                return result;
            }

            return result;
        }


        public static string DeleteF111(F111 selectedF111)
        {
            var result = "Данных не существует";
            using (var tc = new TestContext())
            {
                tc.F111s.Remove(selectedF111);
                tc.SaveChanges();
                result = "Данные успешно удалены";
            }

            return result;
        }

        // метод удаления подразделения
        public static string DeletePodr(Podr selectedPodr)
        {
            var result = "Такого подразделения не существует";

            using (var tc = new TestContext())
            {
                tc.Podrs.Remove(selectedPodr);
                tc.SaveChanges();
                result = "Сделано! Подразделение: " + selectedPodr.NamePodr + " - удалено";
            }

            return result;
        }

        // редактирование наименования подразделения
        public static string
            EditPodr(Podr SelectedPodr, string newNamePodr) // передаем объект Podr и новое наименование
        {
            var result = "такого подразделения не существует";
            using (var db = new TestContext())
            {
                //check user is exist
                var podr = db.Podrs.FirstOrDefault(p => p.Id == SelectedPodr.Id);

                if (podr != null)
                {
                    podr.NamePodr = newNamePodr;

                    db.SaveChanges();
                    result = "Сделано! Нименование изменено на " + podr.NamePodr;
                }
            }

            return result;
        }

        // удаление данных выбранного пользователя
        public static string DeleteUser(User user)
        {
            var result = "Такого сотрудника не существует";
            using (var db = new TestContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
                result = "Внимание! Сотрудник " + user.Lname + " буден удален";
            }

            return result;
        }


        public static string CreatePodr(string namePodr)
        {
            var result = "Уже существует";

            using (var tc = new TestContext())
            {
                var checkIsExist = tc.Podrs.Any(el => el.NamePodr == namePodr);
                if (!checkIsExist)
                {
                    var newPodr = new Podr
                    {
                        NamePodr = namePodr
                    };

                    tc.Podrs.Add(newPodr);
                    tc.SaveChanges();

                    result = "Сделано";
                }

                return result;
            }
        }


        public static string DeleteNO(NameOborud selectedNO)
        {
            var result = "Такого подразделения не существует";

            using (var tc = new TestContext())
            {
                tc.NameOboruds.Remove(selectedNO);
                tc.SaveChanges();
                result = "Сделано! Подразделение: " + selectedNO.NameOborud1 + " - удалено";
            }

            return result;
        }

        // получить все данные таблицы Komplect
        internal static ObservableCollection<Komplect> GetAllKomplect()
        {
            using (var tc = new TestContext())
            {
                var result = tc.Komplects.ToObservableCollection();
                return result;
            }
        }

        // получаем данные в формуляре по IdF111
        internal static ObservableCollection<Formular> GetAllDataFormularToIdF111(int idf111)
        {
            using (var tc = new TestContext())
            {
                //var result = tc.F111s.Find(SelectedRowF111.Id);
                var result = tc.Formulars.Where(u => u.Idf111 == idf111).ToObservableCollection();

                if (result != null) return result;

                MessageBox.Show("Данные отсутствуют");
                return null;
            }
        }

        // добавляем данные в формуляр
        //public static string AddDataFormular(int Idf111, int IdKomplect, string NumForm, string InvNum, string Serial,
        //    string Model, string Count, DateTime DataTo, DateTime DateIn, DateTime? DateOut, string NumAkt,
        //    string YearProd, string GarantyTo, string SelectedKomplect)

        public static string AddDataFormular(int Idf111, int IdKomplect, Formular addForm, string nameKompl)
        {
            var result = "Такое оборудование уже существует в карточке";

            using (var tc = new TestContext())
            {
                try
                {
                    //проверяем наличие вводимых данных в карточке пользователя
                    var checkIsExistFormular = tc.Formulars.Any(el => el.Model == addForm.Model);
                    if (!checkIsExistFormular)
                    {
                        var newFormular = new Formular
                        {
                            Idf111 = Idf111,
                            IdKomplect = IdKomplect, // id наименования комплектующего 
                            NumForm = addForm.NumForm,
                            InvNum = addForm.InvNum,
                            Model = addForm.Model,
                            Count = addForm.Count,
                            Serial = addForm.Serial,
                            DataTo = addForm.DataTo,
                            DateIn = addForm.DateIn,
                            DateOut = addForm.DateOut,
                            NumAkt = addForm.NumAkt,
                            YearProd = addForm.YearProd,
                            GarantyTo = addForm.GarantyTo,
                            NameKomplect = nameKompl
                        };

                        tc.Formulars.Add(newFormular);
                        tc.SaveChanges();

                        result = "Данные успешно добавлены";
                        return result;
                    }
                }
                catch (Exception e)
                {
                }
            }

            return result;
        }


        // редактируем формуляр

        public static string EditFormular(Formular SelectedRowFormular, int IdNameKomp, string NameKompl,
            string NumForm, string InvNum, string Model, string Count, string Serial, DateTime DataTo, DateTime DateIn,
            DateTime? DateOut, string NumAkt, string YearProd, string GarantyTo)
        {
            var result = "Данные не изменены!";
            using (var db = new TestContext())
            {
                //check user is existwa
                var form = db.Formulars.FirstOrDefault(u => u.Id == SelectedRowFormular.Id);

                if (form != null)
                {
                    form.IdKomplect = IdNameKomp;
                    form.NameKomplect = NameKompl;
                    form.NumForm = NumForm;
                    form.InvNum = InvNum;
                    form.Model = Model;
                    form.Count = Count;
                    form.Serial = Serial;
                    form.DataTo = DataTo;
                    form.DateIn = DateIn;
                    form.DateOut = DateOut;
                    form.NumAkt = NumAkt;
                    form.YearProd = YearProd;
                    form.GarantyTo = GarantyTo;
                    //form.NameKomplect = NameKomplect;


                    db.SaveChanges();
                    result = "Сделано! данные успешно  изменены!";
                    return result;
                }

                return result;
            }
        }

        public static string DeleteFormular(Formular selectedRowFormular)
        {
            var result = "Данных не существует";
            using (var tc = new TestContext())
            {
                tc.Formulars.Remove(selectedRowFormular);
                tc.SaveChanges();
                result = "Данные успешно удалены";
            }

            return result;
        }

        public static ObservableCollection<F111> AllPvm()
        {
            using (var tc = new TestContext())
            {
                var result = tc.F111s.ToObservableCollection();
                return result;
            }
        }


        //метод для поиска колличества всего оборудования по категориям
        public static ObservableCollection<int> GetOborudColl()
        {
            var countColl = new ObservableCollection<int>(); // обьявляем новую коллекцию

            using (var tc = new TestContext())
            {
                foreach (var no in tc.NameOboruds.ToObservableCollection())
                {
                    var count = 0;

                    // проверяем по таблице F111s f.IdnameOborud == no.Id т.к. все колличество оборудования храниться там 
                    var idF111 = tc.F111s.Where(f => f.IdnameOborud == no.Id);

                    // подсчитываем колличество одинакового оборудования
                    foreach (var temp in idF111)
                        if (temp.IdnameOborud == no.Id)
                            count++;

                    countColl.Add(count);
                }
            }

            return countColl;
        }

        //метод для нахождения id
        public static CompositeCollection GetAllNameOborudId()
        {
            var NameOborudid = new CompositeCollection();

            using (var tc = new TestContext())
            {
                var no = tc.NameOboruds.ToObservableCollection();

                foreach (var temp in no)
                    NameOborudid.Add(temp.Id);
            }

            return NameOborudid;
        }

        //// метод для нахождения наименования NameOborud1
        public static ObservableCollection<string> GetAllNameOborudNameOb()
        {
            var NameOborudON = new ObservableCollection<string>();

            using (var tc = new TestContext())
            {
                var no = tc.NameOboruds.ToObservableCollection();

                foreach (var temp in no)
                    NameOborudON.Add(temp.NameOborud1);
            }

            return NameOborudON;
        }

        public static CollectionViewSource GetIdNameOborud()
        {
            var source1 = new CollectionViewSource();
            source1.Source = GetAllNameOborudId();
            return source1;
        }


        // метод для объединения коллекций в одну
        public static CompositeCollection UnionColl()
        {
            var source1 = new CollectionViewSource();
            //source1.Source = UnionNameOborud(); //GetAllNameOborud(); //GetAllNameOborud();

            //var source2 = new CollectionViewSource();
            //source2.Source = GetOborudColl(); //GetOborudColl();

            //Объединяем коллекции в составную коллекцию:
            var compositeCollection = new CompositeCollection();

            compositeCollection.Add(new CollectionContainer { Collection = source1.View });
            //compositeCollection.Add(new CollectionContainer { Collection = source2.View });

            return compositeCollection;
        }

        // получить все данные из таблицы OtchetRemont
        public static ObservableCollection<OtchetRemont> GetAllOtchetRemont()
        {
            using (var tc = new TestContext())
            {
                var result = tc.OtchetRemonts.ToObservableCollection();
                return result;
            }
        }

        public static ObservableCollection<F111> GetAllDataF111ToId(int id)
        {
            using (var tc = new TestContext())
            {
                var result = tc.F111s.Where(f => f.IdUser == id);
                return result.ToObservableCollection();
            }
        }

        public static void ChangeRemont(int id, bool val)
        {
            using (var tc = new TestContext())
            {
                var result = tc.F111s.Where(f => f.Id == id);

                foreach (var temp in result) temp.Remont = val;

                tc.SaveChanges();
                //return result.ToObservableCollection();
            }
        }

        public static string AddToRemontDB(OtchetRemont otchetRemont, int id)
        {
            var result = "Косяк";

            using (var tc = new TestContext())
            {
                var otchRem = new OtchetRemont
                {
                    Idf111 = otchetRemont.Idf111,
                    User = otchetRemont.User,
                    Podr = otchetRemont.Podr,
                    InvNum = otchetRemont.InvNum,
                    NumForm = otchetRemont.NumForm,
                    NameOborud = otchetRemont.NameOborud,
                    BeginDate = otchetRemont.BeginDate,
                    EndDate = otchetRemont.EndDate,
                    ZavodNum = otchetRemont.ZavodNum,
                    Title = otchetRemont.Title,
                    TitleComplected = otchetRemont.TitleComplected
                };

                tc.OtchetRemonts.Add(otchRem);
                tc.SaveChanges();

                var temp = tc.F111s.Where(r => r.Id == id).ToObservableCollection(); // нашли выбранный объект F111

                foreach (var column in temp) // нашли столбец Remont и установили ему значение true
                    column.Remont = true;
                tc.SaveChanges();

                result = "Данные добавлены в таблицу Ремонт";
            }

            return result;
        }

        public static string RemoveToRemont(int id)
        {
            var result = "Косяк";

            using (var tc = new TestContext())
            {
                var temp = tc.F111s.Where(r => r.Id == id).ToObservableCollection();

                foreach (var col in temp)
                    col.Remont = false;
                tc.SaveChanges();

                result = "Данные изменены в таблице F111 колонке Remont";
            }


            return result;
        }

        public static string ClearOtchetRemont()
        {
            var result = "Удаление данных из таблицы отчета по ремонту НЕ удалась !!!";

            using (var tc = new TestContext())
            {
                tc.Database.ExecuteSqlRaw("DELETE FROM OtchetRemont");

                var temp = tc.F111s.ToObservableCollection();

                foreach (var column in temp)
                    // нашли столбец Remont и установили ему значение true
                    column.Remont = false;

                tc.SaveChanges();

                MainWindowVM.RefreshF111();

                return result = "Данные из таблицы отчет по ремонту успешно удалены !!!";
            }

            return result;
        }

        // Добавляем в БД Системного пользователя
        public static string AddSysUser(string fname, string login, string pass)
        {
            var result = "Уже существует";

            using (var tc = new TestContext())
            {
                try
                {
                    // проверяем наличие пользователя
                    var chechIsExist = tc.UserSys.Any(el =>
                        el.Fname == fname && el.Login == login);

                    if (!chechIsExist)
                    {
                        var newUser = new UserSy
                        {
                            Fname = fname,
                            Login = login,
                            Pass = pass // СВЯЗЬ FK(users) и PK(podrs)
                        };
                        tc.UserSys.Add(newUser);
                        tc.SaveChanges();


                        result = "Сделано";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                return result;
            }
        }

        public static string CreateSysUser(string fname, string login, string pass)
        {
            var result = "Уже существует";

            using (var tc = new TestContext())
            {
                try
                {
                    // проверяем наличие пользователя
                    var chechIsExist = tc.UserSys.Any(el => el.Fname == fname && el.Login == login && el.Pass == pass);

                    if (!chechIsExist)
                    {
                        var newUser = new UserSy
                        {
                            Fname = fname,
                            Login = login,
                            Pass = pass
                        };
                        tc.UserSys.Add(newUser);
                        tc.SaveChanges();

                        result = "Сделано";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                tc.Dispose();
                return result;
            }
        }

        // загрузить всех системныйх пользователей
        public static ObservableCollection<UserSy> GetAllUserSys()
        {
            using (var tc = new TestContext())
            {
                var result = tc.UserSys.ToObservableCollection();
                return result;
            }
        }

        public static string EditSysUser(UserSy selectedUserSys, string fname, string login, string pass)
        {
            var result = "Данные не изменены!";

            using (var db = new TestContext())
            {
                //check user is exist
                var user = db.UserSys.FirstOrDefault(u => u.Id == selectedUserSys.Id);

                if (user != null)
                {
                    user.Fname = fname;
                    user.Login = login;
                    user.Pass = pass;

                    db.SaveChanges();
                    result = "Сделано! данные успешно  изменены!";
                    return result;
                }

                return result;
            }
        }

        public static string DeleteSysUser(UserSy selectedUserSys)
        {
            var result = "Такого пользователя не существует";

            using (var tc = new TestContext())
            {
                tc.UserSys.Remove(selectedUserSys);
                tc.SaveChanges();
                result = "Сделано! Пользователь: " + selectedUserSys.Fname + " - удален";
            }

            return result;
        }

        public static bool Registration(string login, string pass)
        {
            using (var tc = new TestContext())
            {
                tc.UserSys.ToObservableCollection();

                foreach (var temp in tc.UserSys)
                    if ((temp.Login == login) & (temp.Pass == pass))
                        return true;
            }

            return false;
        }
    }
}