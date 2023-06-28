using System.Collections.ObjectModel;

namespace WpfEfCoreTest.Model.Data
{
    public class UnionColl
    {
        public ObservableCollection<int> CountOborudColl { get; set; }

        public ObservableCollection<NameOborud> AllOborud { get; set; }
    }
}