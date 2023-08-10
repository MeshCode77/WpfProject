using System.Windows.Controls;

namespace WpfEfCoreTest.Model.Data
{
    public static class DataTransfer
    {
        public static ListView AllDataF111View;

        #region Свойства для F111

        // свойства для таблицы F111
        public static int IdF111 { get; set; }

        public static int IdUser { get; set; }

        public static string FilterNK { get; set; } // строка фильтр для поиска наименования комплектующего

        #endregion
    }
}