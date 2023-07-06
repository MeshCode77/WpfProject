using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using WpfEfCoreTest.Model;

//https://metanit.com/sharp/wpf/22.6.php

namespace WpfEfCoreTest
{
    public class JsonFileService : IFileService
    {
        public List<OtchetRemont> Open(string filename)
        {
            var remonts = new List<OtchetRemont>();
            var jsonFormatter =
                new DataContractJsonSerializer(typeof(List<OtchetRemont>));
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                remonts = jsonFormatter.ReadObject(fs) as List<OtchetRemont>;
            }

            return remonts;
        }

        public void Save(string filename, List<OtchetRemont> remontsList)
        {
            var jsonFormatter =
                new DataContractJsonSerializer(typeof(List<OtchetRemont>));
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, remontsList);
            }
        }
    }
}