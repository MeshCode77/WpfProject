using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SqlServMvvmApp;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest.ViewModel
{
    public class ComputerScannerVM : INotifyPropertyChanged
    {
        private ObservableCollection<ComputerComponent> _components;
        private string _hostName;

        public string _ipAdress;
        private ObservableCollection<ScanHost> _scanHostColl;
        private string _status;

        private string hostName;

        public RelayCommand scanCommand;

        public ComputerScannerVM()
        {
            var ScanHostColl = new ObservableCollection<ScanHost>();
        }


        public string IpAdress
        {
            get => _ipAdress;
            set
            {
                _ipAdress = value;
                OnPropertyChanged(nameof(IpAdress));
            }
        }

        public string HostName
        {
            get => _hostName;
            set
            {
                _hostName = value;
                OnPropertyChanged(nameof(HostName));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


        public ObservableCollection<ScanHost> ScanHostColl
        {
            get => _scanHostColl;
            set
            {
                _scanHostColl = value;
                OnPropertyChanged(nameof(ScanHostColl));
            }
        }

        public ObservableCollection<ComputerComponent> Components
        {
            get => _components;
            set
            {
                _components = value;
                OnPropertyChanged("Components");
            }
        }


        public RelayCommand ScanCommand
        {
            get
            {
                return scanCommand ?? new RelayCommand(obj =>
                {
                    ScanHostColl = ScanNetwork();

                    // GetHostNameByIpAddress(IpAdress);
                    //PingHost();
                    //ScanNetworkAsync("192.168.100.150");
                    //ScanLocalNetworkAsync();
                });
            }
        }

        public string GetHostNameByIpAddress(string ipAddress)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(ipAddress);

                //MessageBox.Show(string.Format("IP адресс:{0}\n Имя хоста:{1}", ipAddress, hostEntry.HostName));
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                return null;
            }
        }

        public ObservableCollection<ScanHost> ScanNetwork()
        {
            var ScanHostColl = new ObservableCollection<ScanHost>();
            // Очищаем список NetworkInterfaces перед сканированием
            //ScanHostColl.Clear();

            // Получаем все сетевые интерфейсы
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Проходим по всем интерфейсам и получаем информацию
            foreach (var ni in interfaces)
                // Отбираем только необходимые сетевые интерфейсы
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    // Получаем IP-адреса для текущего интерфейса
                    var ipProps = ni.GetIPProperties();
                    foreach (var addr in ipProps.UnicastAddresses)
                        // Проверяем, что это IPv4-адрес
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            // Получаем имя хоста по IP-адресу
                            var hostName = GetHostNameByIpAddress(addr.Address.ToString());

                            // Создаем экземпляр ViewModel для NetworkInterface
                            var niViewModel = new ScanHost
                            {
                                IpAdress = addr.Address.ToString(),
                                HostName = hostName,
                                Status = "Active"
                            };

                            ScanHostColl.Add(niViewModel);
                        }
                }

            return ScanHostColl;
        }


        public void PingHost()
        {
            var scan = new ScanHost();

            var worker = new BackgroundWorker();

            Thread.Sleep(500);

            PingReply pingReply;
            string name;


            //string name;
            var ping = new Ping();
            var reply = ping.Send(IpAdress, 1000);
            IPAddress addr;
            IPHostEntry host;

            addr = IPAddress.Parse(scan.IpAdress);
            //host = Dns.GetHostEntry();
            //name = host.HostName;
            //ScanHostColl.Add(reply.Address.ToString());
            //MessageBox.Show(reply.Address + " " + reply.Status);
        }

        public async Task ScanLocalNetworkAsync()
        {
            var components = new List<ComputerComponent>();

            var hostName = Dns.GetHostName();
            var currentIpAddress = ""; //"192.168.100.150";

            var hostEntry = Dns.GetHostEntry(hostName);
            foreach (var ipAddress in hostEntry.AddressList)
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    currentIpAddress = ipAddress.ToString();
                    break;
                }

            var ipOctets = currentIpAddress.Split('.');
            var baseIpAddress = $"{ipOctets[0]}.{ipOctets[1]}.{ipOctets[2]}.";

            for (var i = 1; i < 256; i++)
            {
                var ipAddress = $"{baseIpAddress}{i}";

                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(ipAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        // Ваш код для сбора информации о компонентах компьютера в локальной сети
                        // Пример:
                        //components.Add(new ComputerComponent
                        //    { Name = "Processor", Manufacturer = "Intel", Description = "Intel Core i7" });
                        //components.Add(new ComputerComponent
                        //    { Name = "Motherboard", Manufacturer = "ASUS", Description = "ASUS Prime Z390" });
                        //components.Add(new ComputerComponent
                        //{
                        //    Name = "Graphics Card", Manufacturer = "NVIDIA", Description = "NVIDIA GeForce RTX 2080 Ti"
                        //});
                    }
                }
            }

            Components = new ObservableCollection<ComputerComponent>(components);
        }


        public async Task ScanNetworkAsync(string ipAddress)
        {
            var components = new List<ComputerComponent>();

            using (var ping = new Ping())
            {
                var reply = await ping.SendPingAsync(ipAddress);
                if (reply.Status == IPStatus.Success)
                {
                    // Ваш код для сбора информации о компонентах удаленного компьютера
                    // Пример:
                    components.Add(new ComputerComponent
                        { Name = "Processor", Manufacturer = "Intel", Description = "Intel Core i7" });
                    components.Add(new ComputerComponent
                        { Name = "Motherboard", Manufacturer = "ASUS", Description = "ASUS Prime Z390" });
                    components.Add(new ComputerComponent
                    {
                        Name = "Graphics Card", Manufacturer = "NVIDIA", Description = "NVIDIA GeForce RTX 2080 Ti"
                    });
                }
            }

            Components = new ObservableCollection<ComputerComponent>(components);
        }


        #region Реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}