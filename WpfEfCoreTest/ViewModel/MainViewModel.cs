using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using SqlServMvvmApp;
using WpfEfCoreTest.Annotations;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest.ViewModel
{
    public class MainViewModel : INotifyCollectionChanged
    {
        private ObservableCollection<ScanHost> _scanResult;

        public MainViewModel()
        {
            Progress = new Progress<int>(value =>
            {
                // обновляем значение прогрессбара
                if (MyProgressBar != null) MyProgressBar.Value = value;
            });

            ScanNetworkCommand = new RelayCommand(async async  => await ScanNetworkWithProgressAsync(Progress));
        }


        public RelayCommand ScanNetworkCommand { get; }

        public ObservableCollection<ScanHost> ScanResult
        {
            get => _scanResult;
            set
            {
                _scanResult = value;
                OnPropertyChanged();
            }
        }

        private Progress<int> Progress { get; }

        private ProgressBar MyProgressBar { get; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private async Task ScanNetworkWithProgressAsync(IProgress<int> progress)
        {
            var baseAddress = "192.168.100.";
            var tasks = new List<Task<ScanHost>>();

            for (var i = 148; i < 152; i++)
            {
                var scanHost = new ScanHost
                {
                    Status = "Inactive",
                    IpAdress = baseAddress + i
                };

                tasks.Add(Task.Run(async () =>
                {
                    using (var ping = new Ping())
                    {
                        var pingReply = await ping.SendPingAsync(scanHost.IpAdress, 1000);
                        if (pingReply.Status == IPStatus.Success) scanHost.Status = "Active";
                    }

                    scanHost.HostName = await FetchHostNameAsync(scanHost.IpAdress);
                    return scanHost;
                }));
            }

            var result = await Task.WhenAll(tasks);
            ScanResult = new ObservableCollection<ScanHost>(result);
        }

        private async Task<string> FetchHostNameAsync(string ipAddress)
        {
            try
            {
                var hostEntry = await Dns.GetHostEntryAsync(ipAddress);
                return hostEntry.HostName;
            }
            catch
            {
                return "Unknown";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}