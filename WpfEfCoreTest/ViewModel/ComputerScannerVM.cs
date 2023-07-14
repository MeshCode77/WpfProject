using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SqlServMvvmApp;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest.ViewModel
{
    public class ComputerScannerVM : INotifyPropertyChanged
    {
        public delegate ObservableCollection<ScanHost> ThreadStart();

        private ObservableCollection<ComputerComponent> _components;
        private string _hostName;

        public string _ipAdress;

        private int _isScanning;
        private ObservableCollection<ScanHost> _scanHostColl;

        private ScanHost _selectedHost;
        private string _status;

        private string hostName;

        public RelayCommand scanCommand;

        public ComputerScannerVM()
        {
            var ScanHostColl = new ObservableCollection<ScanHost>();

            var progress = new Progress<int>(value =>
            {
                // обновляем значение прогрессбара
                IsScanning = value;
            });
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

        public ScanHost SelectedHost
        {
            get => _selectedHost;
            set
            {
                _selectedHost = value;
                OnPropertyChanged(nameof(SelectedHost));
                //MessageBox.Show(SelectedHost.IpAdress);
                GetComputerInfoByIpAddress(SelectedHost.IpAdress);
            }
        }

        public int IsScanning
        {
            get => _isScanning;
            set
            {
                if (_isScanning != value)
                {
                    _isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                }
            }
        }

        public RelayCommand ScanCommand
        {
            get
            {
                return scanCommand ?? new RelayCommand(obj =>
                {
                    /// код для создания второго потока
                    var thread = new Thread(() =>
                    {
                        // Код для выполнения во втором потоке
                        ScanHostColl = ScanNetwork2(); //ScanNetwork2();
                    });
                    //thread.IsBackground = true;
                    // Запуск второго потока
                    thread.Start();
                });
            }
        }


        public string GetHostNameByIpAddress(string ipAddress)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                return null;
            }
        }


        public void GetComputerInfoByIpAddress(string ipAddress)
        {
            try
            {
                // Получаем экземпляр класса Ping и отправляем эхо-запрос
                using (var ping = new Ping())
                {
                    var reply = ping.Send(ipAddress);

                    if (reply.Status != IPStatus.Success)
                        // Если устройство не отвечает на эхо-запрос, генерируем исключение
                        throw new Exception("Устройство не найдено");
                }

                // Получаем информацию о хосте
                var hostEntry = Dns.GetHostEntry(ipAddress);
                var hostName = hostEntry.HostName; // Имя хоста
                var addresses = hostEntry.AddressList; // Список IP-адресов, связанных с хостом

                // Получаем информацию об устройстве, соответствующем указанному IP-адресу
                var scope = new ManagementScope("\\\\" + ipAddress + "\\root\\cimv2");

                // Запрашиваем информацию о процессоре
                var query = new ObjectQuery("SELECT * FROM Win32_Processor");
                var searcher = new ManagementObjectSearcher(scope, query);
                var queryCollection = searcher.Get();

                foreach (var o in queryCollection)
                {
                    var m = (ManagementObject)o;
                    var cpu = m["Name"].ToString();
                    var cpuCores = m["NumberOfCores"].ToString();
                    var cpuThreads = m["ThreadCount"].ToString();
                    var cpuArchitecture = m["Architecture"].ToString();

                    MessageBox.Show("Процессор: " + cpu);
                }

                // Запрашиваем информацию о материнской плате
                query = new ObjectQuery("SELECT * FROM Win32_BaseBoard");
                searcher = new ManagementObjectSearcher(scope, query);
                queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    var motherboard = m["Product"].ToString();

                    MessageBox.Show("Материнская плата: " + motherboard);
                }

                // Запрашиваем информацию о жестком диске
                query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
                searcher = new ManagementObjectSearcher(scope, query);
                queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    var hdd = m["Caption"].ToString();

                    MessageBox.Show("Жесткий диск: " + hdd);
                }


                //// Получаем информацию о сетевых интерфейсах
                //foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                //    if ((ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                //         ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                //        ni.OperationalStatus == OperationalStatus.Up)
                //    {
                //        var ipProps = ni.GetIPProperties();
                //        foreach (var addr in ipProps.UnicastAddresses)
                //            if (addr.Address.ToString() == ipAddress)
                //            {
                //                var macAddress = ni.GetPhysicalAddress(); // MAC-адрес
                //                var interfaceDescription = ni.Description; // Описание интерфейса
                //                var interfaceSpeed = (int)(ni.Speed / 1000000); // Скорость интерфейса (Мбит/с)
                //                break;
                //            }
                //    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //

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

        public ObservableCollection<ScanHost> ScanNetwork2() // 
        {
            var sb = new StringBuilder();
            var ScanHostColl = new ObservableCollection<ScanHost>();
            var temp = "192.168.100.";

            var lock_object = new object();

            lock (lock_object)
            {
                // Код для выполнения во втором потоке
                for (var i = 0; i < 153; i++)
                {
                    // Получаем имя хоста по IP-адресу
                    var hostName = GetHostNameByIpAddress(sb.Append(temp + i).ToString());

                    // проверяем хосты на доступность
                    var ping = new Ping();
                    var reply = ping.Send(temp + i, 1000);
                    if (reply.Status == IPStatus.Success)
                    {
                        Status = "Active";
                        // Создаем экземпляр ViewModel для NetworkInterface
                        var niViewModel = new ScanHost
                        {
                            IpAdress = temp + i,
                            HostName = hostName,
                            Status = Status
                        };
                        ScanHostColl.Add(niViewModel);
                        hostName = "";
                    }
                    else
                    {
                        Status = "InActive";
                    }

                    IsScanning++;
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