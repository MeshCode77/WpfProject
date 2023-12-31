﻿using System.Windows.Controls;

namespace WpfEfCoreTest.Model.Data
{
    public static class DataTransfer
    {
        public static ListView AllDataF111View;


        #region Свойства для F111

        // свойства для таблицы F111
        public static int IdF111 { get; set; }

        public static int IdUser { get; set; }

        public static string FilterNO { get; set; } // строка фильтр для поиска наименования оборудования

        public static string FilterNK { get; set; } // строка фильтр для поиска наименования комплектующего

        public static string FilterUser { get; set; } // строка фильтр для поиска фамилии пользователя

        public static string FilterPodr { get; set; } // строка фильтр для поиска подразделения

        #endregion
    }
}