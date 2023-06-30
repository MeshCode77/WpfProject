using System.Collections.Generic;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest
{
    public interface IFileService
    {
        List<OtchetRemont> Open(string filename); // для открытия файла

        void Save(string filename,
            List<OtchetRemont> remontsList); // метод сохраняет данные из списка в файле по определенному пути
    }
}