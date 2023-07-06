using System.Collections.Generic;
using WpfEfCoreTest.Model;

//https://metanit.com/sharp/wpf/22.6.php

namespace WpfEfCoreTest
{
    public interface IFileService
    {
        List<OtchetRemont> Open(string filename); // для открытия файла

        void Save(string filename,
            List<OtchetRemont> remontsList); // метод сохраняет данные из списка в файле по определенному пути
    }
}