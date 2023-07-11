using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class ScanHost : INotifyCollectionChanged
    {
        public string HostName { get; set; }
        public string IpAdress { get; set; }
        public string Status { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}