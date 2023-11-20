using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using server;
using SqlServMvvmApp;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest.ViewModel
{
    public class ComputerScannerVM : INotifyPropertyChanged
    {
        public delegate ObservableCollection<ScanHost> ThreadStart();

        private string _baseBoard;

        private ObservableCollection<ComputerComponent> _components = new();
        private string _diskDrive;

        // Коллекция, которая будет содержать все жесткие диски системы
        private string _hostName;
        public string _ipAdress;


        private CancellationTokenSource cancellationTokenSource;

        private int _isScanning;
        private int _scanningIpSize = 255;
        private string _currentScanningIpAdress;

        private string _processor;
        public string _ramm;
        private ObservableCollection<ScanHost> _scanHostColl = new();

        private ScanHost _selectedHost;
        private string _status;
        public string _vc;

        private string hostName;

        public RelayCommand scanCommand;

        public ComputerScannerVM()
        {
            cancellationTokenSource = new CancellationTokenSource();
            ScanHostColl.CollectionChanged += ScanHostColl_CollectionChanged;
        }

        private async void ScanHostColl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
    
                    if(e.NewItems[0] is ScanHost newItem)

                    await Task.Run(async () =>
                    {
                        newItem.IsBusy = true;

                        var hostName = await GetHostNameByIpAddress(newItem.IpAdress);
                        newItem.HostName = hostName;

                        await Task.Run(() =>
                        {
                            try
                            {
                                if (cancellationTokenSource.IsCancellationRequested)
                                    cancellationTokenSource.Token.ThrowIfCancellationRequested(); // генерируем исключение

                                // Создаем сокет
                                using var client = new TcpClient(newItem.IpAdress, 7000);
                                //Console.WriteLine("Клиент подключен");
                                newItem.IsRemoteAppActive = true;

                                client.Close();
                            }
                            catch
                            {
                                newItem.IsRemoteAppActive = false;
                            }
                        });

                        newItem.IsBusy = false;
                    });

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
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
                OnPropertyChanged(nameof(Components));
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
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
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

        public int ScanningIpSize
        {
            get => _scanningIpSize;
            set
            {
                if (_scanningIpSize != value)
                {
                    _scanningIpSize = value;
                    OnPropertyChanged(nameof(ScanningIpSize));
                }
            }
        }

        public string CurrentScanningIpAdress
        {
            get => _currentScanningIpAdress;
            set
            {
                if (_currentScanningIpAdress != value)
                {
                    _currentScanningIpAdress = value;
                    OnPropertyChanged(nameof(CurrentScanningIpAdress));
                }
            }
        }

        public RelayCommand ScanCommand
        {
            get
            {
                return scanCommand ?? new RelayCommand(async obj =>
                {
                    try
                    {
                        cancellationTokenSource = new CancellationTokenSource();

                        IsBusy = true;

                        ScanHostColl.Clear();

                        //var temp = "192.168.100";
                        /// код для создания второго потока
                        //var task = Task.Run(async () =>
                        //{
                        //    //var temp = "192.168.100";
                        //    //ScanHostColl = await ScanNetwork3());
                        //    return ScanHostColl = new ObservableCollection<ScanHost>();
                        //});
                        using var ping = new Ping();

                        var startIps = StartIpAdress.Split(".").ToList();
                        var nextIps = NextIpAdress.Split(".").ToList();

                        if (startIps.Count == 4 && nextIps.Count == 4 && nextIps.Count == startIps.Count && string.Join("", startIps.Take(3)) == string.Join("", nextIps.Take(3)))
                        {
                            IsScanning = 1;
                            ScanningIpSize = int.Parse(nextIps[3]) - int.Parse(startIps[3]);

                            for (var i = int.Parse(startIps[3]); i <= int.Parse(nextIps[3]); i++)
                            {
                                if (cancellationTokenSource.IsCancellationRequested)
                                    cancellationTokenSource.Token.ThrowIfCancellationRequested(); // генерируем исключение

                                CurrentScanningIpAdress = $"{startIps[0]}.{startIps[1]}.{startIps[2]}.{i}";

                                var reply1 = await ping.SendPingAsync(CurrentScanningIpAdress, 1);

                                if (reply1.Status == IPStatus.Success)
                                {
                                    //var hostName = GetHostNameByIpAddress(ipAddress);

                                    var niViewModel = new ScanHost
                                    {
                                        IpAdress = CurrentScanningIpAdress,
                                        //HostName = hostName.Result,
                                        Status = reply1.Status.ToString()
                                    };

                                    // Добавляем хост в коллекцию на основном потоке
                                    //Application.Current.Dispatcher.Invoke(() => { ScanHostColl.Add(niViewModel); });
                                    ScanHostColl.Add(niViewModel);
                                    OnPropertyChanged(nameof(ScanHostColl));
                                }

                                IsScanning++;
                            }
                        }

                        CurrentScanningIpAdress = string.Empty;
                        IsBusy = false;
                    }
                    catch (OperationCanceledException ae)
                    {
                        IsScanning = 1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Ошибка");
                    }
                    finally
                    {
                        cancellationTokenSource.Dispose();
                        IsBusy = false;
                        CurrentScanningIpAdress = string.Empty;
                    }

                    // Запуск второго потока
                    //thread.Start();
                }, obj => {

                    if (IsBusy) return false;

                    if (string.IsNullOrEmpty(StartIpAdress) || string.IsNullOrEmpty(NextIpAdress)) return false;

                    var startIps = StartIpAdress.Split(".").ToList();
                    var nextIps = NextIpAdress.Split(".").ToList();

                    if(startIps.Count != 4) return false;
                    if (startIps.Count != 4) return false;

                    if(nextIps.Count != startIps.Count) return false;

                    if(string.Join("", startIps.Take(3)) != string.Join("", nextIps.Take(3))) return false;

                    if(string.IsNullOrEmpty(startIps[3]) || string.IsNullOrEmpty(nextIps[3])) return false;


                    if(!int.TryParse(startIps[3], out var startIp)) return false;
                    if(!int.TryParse(nextIps[3], out var nextIp)) return false;


                    if(nextIp > 255) return false;

                    if(startIp >= nextIp) return false;

                    return true;
                });
            }
        }

        public RelayCommand StopScanCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    cancellationTokenSource.Cancel();

                }, obj => IsBusy);
            }
        }

        public RelayCommand GetCompInfoCommand
        {
            get
            {
                return new RelayCommand(async obj =>
                {
                    if (SelectedHost != null)
                    {
                        try
                        {
                            SelectedHost.IsBusy = true;
                            await GetComputerInfoByIpAddress(SelectedHost.IpAdress);
                            
                            Components.Clear();

                            var component = new ComputerComponent
                            {
                                Processor = Processor,
                                BaseBoard = BaseBoard,
                                DiskDrive = DiskDrive,
                                Videocard = Videocard,
                                Ramm = Ramm
                            };

                            Components.Add(component);

                            SelectedHost.IsRemoteAppActive = true;
                        }
                        catch (Exception ex)
                        {
                            SelectedHost.IsRemoteAppActive = false;
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            SelectedHost.IsBusy = false;
                        }
                    }

                }, obj =>
                {
                    //if (IsBusy) return false;
                    if(SelectedHost == null) return false;
                    if(SelectedHost.IsBusy) return false;

                    return true;
                });
            }
        }

        public ObservableCollection<string> HardDrivesColl { get; set; } = new();


        public async Task<string> GetHostNameByIpAddress(string ipAddress)
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

        public async Task GetComputerInfoByIpAddress(string ipAddress)
        {
            // Создаем сокет
            using var client = new TcpClient(ipAddress, 7000);
            //Console.WriteLine("Клиент подключен");

            // Отправляем команду
            using var stream = client.GetStream();
            var request = "get_info";
            var bytesWrite = Encoding.UTF8.GetBytes(request);
            await stream.WriteAsync(bytesWrite, 0, bytesWrite.Length);
            stream.Flush();
            //Console.WriteLine("Запрос на сервер: " + request); // + bytesWrite.Length);

            // Получаем ответ

            var buffer = new byte[4096];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            // Преобразуем полученные данные из байтового массива в строку JSON
            var info = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            //Console.WriteLine(answer);

            // Десериализуем строку JSON в объекты C#
            var cc = new Components();

            cc = JsonConvert.DeserializeObject<Components>(info);

            Processor = cc.Processor;
            BaseBoard = cc.BaseBoard;
            DiskDrive = cc.DiskDrive;
            Videocard = cc.Videocard;
            Ramm = cc.Ramm;

            client.Close();


            // Получаем экземпляр класса Ping и отправляем эхо-запрос
            using (var ping = new Ping())
            {
                var reply = await ping.SendPingAsync(ipAddress,1);

                if (reply.Status != IPStatus.Success)
                    // Если устройство не отвечает на эхо-запрос, генерируем исключение
                    throw new Exception("Устройство не найдено");
            }

            // Получаем информацию о хосте
            // TODO не понимаю зачем тут опрашивать DNS имя и список адресов
            //var hostEntry = Dns.GetHostEntry(ipAddress);
            //var HostName = hostEntry.HostName; // Имя хоста
            //var addresses = hostEntry.AddressList; // Список IP-адресов, связанных с хостом

            await Task.Run(() =>
            {
                // Получаем информацию об устройстве, соответствующем указанному IP-адресу
                var scope = new ManagementScope("\\\\" + ipAddress + "\\root\\cimv2");

                // Запрашиваем информацию о процессоре
                var query = new ObjectQuery("SELECT * FROM Win32_Processor");
                var searcher = new ManagementObjectSearcher(scope, query);
                var queryCollection = searcher.Get();

                //foreach (var o in queryCollection)
                //{
                //    var m = (ManagementObject)o;
                //    //var cpu = m["Name"].ToString();
                //    var cpuCores = m["NumberOfCores"].ToString();
                //    var cpuThreads = m["ThreadCount"].ToString();
                //    var cpuArchitecture = m["Architecture"].ToString();
                //Processor = cpuInfo;

                //    //MessageBox.Show("Процессор: " + cpu);
                //}


                // Запрашиваем информацию о материнской плате
                //var queryMB = new ObjectQuery("SELECT * FROM Win32_BaseBoard");
                //var searcherMB = new ManagementObjectSearcher(scope, queryMB);
                //var queryCollectionMB = searcherMB.Get();

                //foreach (ManagementObject m in queryCollectionMB)
                //{
                //    var motherboard = m["Product"].ToString();
                //    //memoryType = m["MemoryType"].ToString();

                //    //MessageBox.Show("Материнская плата: " + motherboard);
                //    BaseBoard = motherboard;
                //}

                // Запрашиваем информацию о жестком диске
                query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
                searcher = new ManagementObjectSearcher(scope, query);
                queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    var hdd = m["Caption"].ToString();

                    //MessageBox.Show("Жесткий диск: " + hdd);
                    DiskDrive = hdd;

                    HardDrivesColl.Add(DiskDrive);
                }

                // Запрашиваем информацию о видеокарте
                query = new ObjectQuery("SELECT * FROM Win32_VideoController");
                searcher = new ManagementObjectSearcher(scope, query);
                queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
                    var vc = m["Caption"].ToString();

                    //MessageBox.Show("Жесткий диск: " + hdd);
                    Videocard = vc;
                }


                // Запрашиваем информацию о оперативной памяти
                var memoryType = "";
                var mt = "";
                ulong memorySize = 0;

                // Create WMI query to get memory information
                query = new ObjectQuery("SELECT * FROM CIM_PhysicalMemory");
                searcher = new ManagementObjectSearcher(scope, query);
                queryCollection = searcher.Get();

                //// Get memory information
                foreach (var o in queryCollection)
                {
                    var m = (ManagementObject)o;
                    memoryType = m["MemoryType"].ToString();
                    memorySize += Convert.ToUInt64(m["Capacity"]);
                }


                //if (memoryType == "21")
                //    mt = "DDR2";
                //if (memoryType == "24")
                //    mt = "DDR3";
                //if (memoryType == "26")
                //    mt = "DDR4";

                // Convert memory size from bytes to gigabytes
                var memorySizeGB = memorySize / Math.Pow(1024, 3);
                var result = $"Тип памяти: {memoryType}, объем памяти: {memorySizeGB:F2} ГБ";
                //var result = $"Объем памяти: {memorySizeGB:F2} ГБ";
                Ramm = result;
            });
        }


        public ObservableCollection<ComputerComponent> AddCompComponent()
        {
            var components = new ObservableCollection<ComputerComponent>();

            var component = new ComputerComponent
            {
                Processor = Processor,
                BaseBoard = BaseBoard,
                DiskDrive = DiskDrive,
                Videocard = Videocard,
                Ramm = Ramm
            };

            components.Add(component);
            OnPropertyChanged(nameof(Components));

            return Components;
        }

        #region Свойства класса

        public string DiskDrive
        {
            get => _diskDrive;
            set
            {
                _diskDrive = value;
                OnPropertyChanged(nameof(DiskDrive));
            }
        }


        public string BaseBoard
        {
            get => _baseBoard;
            set
            {
                _baseBoard = value;
                OnPropertyChanged(nameof(BaseBoard));
            }
        }

        public string Processor
        {
            get => _processor;
            set
            {
                _processor = value;
                OnPropertyChanged(nameof(Processor));
            }
        }

        public string Videocard
        {
            get => _vc;
            set
            {
                _vc = value;
                OnPropertyChanged(nameof(Videocard));
            }
        }

        public string Ramm
        {
            get => _ramm;
            set
            {
                _ramm = value;
                OnPropertyChanged(nameof(Ramm));
            }
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

        private string _startIpAdress;

        public string StartIpAdress
        {
            get => _startIpAdress;
            set
            {
                _startIpAdress = value;
                OnPropertyChanged(nameof(StartIpAdress));
            }
        }

        private string _nextIpAdress;

        public string NextIpAdress
        {
            get => _nextIpAdress;
            set
            {
                _nextIpAdress = value;
                OnPropertyChanged(nameof(NextIpAdress));
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

        #endregion


        #region Реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}