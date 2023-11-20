using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfEfCoreTest.Annotations;

namespace WpfEfCoreTest.Model
{
    public class ScanHost : INotifyPropertyChanged
    {
        private string _hostName;

        public string HostName
        {
            get => _hostName;
            set
            {
                _hostName = value;
                OnPropertyChanged(nameof(HostName));
            }
        }
        public string IpAdress { get; set; }
        public string Status { get; set; }


        private bool _isRemoteAppActive;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRemoteAppActive
        {
            get => _isRemoteAppActive;
            set
            {
                _isRemoteAppActive = value;
                OnPropertyChanged(nameof(IsRemoteAppActive));
            }
        }

        private bool _isBusy;

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        //public event NotifyCollectionChangedEventHandler CollectionChanged;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}