﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
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
                        ScanHostColl = ScanNetwork2Async().Result);
                    OnPropertyChanged(nameof(ScanHostColl));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public async Task<ObservableCollection<ScanHost>> ScanNetwork2Async()
        {
            var sb = new StringBuilder();
            var temp = "192.168.100";
            //var i = 0;
            ScanHost niViewModel = null; // = new ScanHost
            var ScanHostColl = new ObservableCollection<ScanHost>();

            for (var i = 0; i < 255; i++)
            {
                await Task.Run(async () =>
                {
                    var ipAddress = $"{temp}.{i}";
                    //var status = reply.Status == IPStatus.Success ? "Active" : "InActive";
                    var hostName = GetHostNameByIpAddress(sb.Append(temp + i).ToString());
                    var ping = new Ping();
                    //var reply = ping.Send(temp + i, 1000);
                    var reply1 = await ping.SendPingAsync(ipAddress, 1000);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (reply1.Status == IPStatus.Success)
                        {
                            Status = "Active";
                            niViewModel = new ScanHost
                            {
                                IpAdress = ipAddress,
                                HostName = hostName,
                                Status = reply1.Status.ToString()
                            };
                            ScanHostColl.Add(niViewModel);
                            OnPropertyChanged(nameof(ScanHostColl));
                        }
                        else
                        {
                            Status = "InActive";
                        }
                    });
                    IsScanning++;
                });

                OnPropertyChanged(nameof(ScanHostColl));
            }

            return ScanHostColl;
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