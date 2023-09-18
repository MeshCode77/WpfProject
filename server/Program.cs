using System;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using server;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Принимаем подключение
                var serverSocket = new TcpListener(IPAddress.Any, 7000);
                Console.WriteLine("Сервер запущен");
                serverSocket.Start();
                while (true)
                {
                    var clientSocket = serverSocket.AcceptTcpClient();
                    var stream = clientSocket.GetStream();

                    // Читаем команду
                    var bytes = new byte[4096];
                    var lenght = stream.Read(bytes, 0, bytes.Length);
                    var request = Encoding.UTF8.GetString(bytes, 0, lenght);

                    var cpu = "";
                    var mb = "";
                    var hdd = "";
                    var vc = "";
                    var memorySizeGb = 0;

                    Console.WriteLine("Получен запрос: " + request);

                    // Обрабатываем команду
                    if (request == "get_info")
                    {
                        // код для получения инфы о ЦП
                        // Получаем информацию об устройстве, соответствующем указанному IP-адресу
                        var scope = new ManagementScope("\\\\" + "127.0.0.1" + "\\root\\cimv2");

                        // Запрашиваем информацию о процессоре
                        var query = new ObjectQuery("SELECT * FROM Win32_Processor");
                        var searcher = new ManagementObjectSearcher(scope, query);
                        var queryCollection = searcher.Get();

                        foreach (var o in queryCollection)
                        {
                            var m = (ManagementObject)o;
                            cpu = m["Name"].ToString();
                        }

                        // Запрашиваем информацию о материнской плате
                        var queryMB = new ObjectQuery("SELECT * FROM Win32_BaseBoard");
                        var searcherMB = new ManagementObjectSearcher(scope, queryMB);
                        var queryCollectionMB = searcherMB.Get();

                        foreach (ManagementObject m in queryCollectionMB)
                            mb = m["Product"].ToString();


                        // Запрашиваем информацию о жестком диске
                        var queryHDD = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
                        searcher = new ManagementObjectSearcher(scope, queryHDD);
                        queryCollection = searcher.Get();

                        foreach (ManagementObject m in queryCollection)
                            hdd = m["Caption"].ToString();

                        //HardDrivesColl.Add(DiskDrive);


                        // Запрашиваем информацию о видеокарте
                        query = new ObjectQuery("SELECT * FROM Win32_VideoController");
                        searcher = new ManagementObjectSearcher(scope, query);
                        queryCollection = searcher.Get();

                        foreach (ManagementObject m in queryCollection)
                            vc = m["Caption"].ToString();


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
                        memorySizeGb = (int)(memorySize / Math.Pow(1024, 3));
                        //var result = $"Тип памяти: {memoryType}, объем памяти: {memorySizeGB:F2} ГБ";
                        //var result = $"Объем памяти: {memorySizeGB:F2} ГБ";
                    }


                    // Отправляем ответ
                    var comp = new Components();

                    comp.Processor = cpu;
                    comp.BaseBoard = mb;
                    comp.DiskDrive = hdd;
                    comp.Videocard = vc;
                    comp.Ramm = memorySizeGb.ToString();

                    var json = JsonConvert.SerializeObject(comp);

                    var message = string.Format("{0}\n {1}\n", cpu, mb);
                    bytes = Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();

                    Console.WriteLine("Ответ сервера:\n " + message);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}