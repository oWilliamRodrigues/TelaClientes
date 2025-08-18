using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace Europa.Diagnostics
{
    public class SystemInfo
    {
        public Dictionary<string, object> GetSystemInfo()
        {
            var SOInfo = GetSOInfo();
            var drivesInfo = GetDrivesInfo();
            var networkInfo = GetNetworkInfo();
            var processorInfo = GetProcessorInfo();
            SOInfo.ToList().ForEach(x => drivesInfo.Add(x.Key, x.Value));
            networkInfo.ToList().ForEach(x => drivesInfo.Add(x.Key, x.Value));
            processorInfo.ToList().ForEach(x => drivesInfo.Add(x.Key, x.Value));

            return drivesInfo;
        }

        private Dictionary<string, object> GetSOInfo()
        {
            var wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            var searcher = new ManagementObjectSearcher(wql);
            var results = searcher.Get();
            var resultDict = new Dictionary<string, Object>();

            foreach (var item in results)
            {
                foreach (var teste in item.Properties)
                {
                    Console.WriteLine(teste.Name + " | " + teste.Value + " | " + teste.Type);

                }

                resultDict.Add("MemoriaFisicaTotal", (UInt64)item["TotalVisibleMemorySize"] / 1024);
                resultDict.Add("MemoriaFisicaLivre", (UInt64)item["FreePhysicalMemory"] / 1024);
                resultDict.Add("MemoriaVirtualTotal", (UInt64)item["TotalVirtualMemorySize"] / 1024);
                resultDict.Add("MemoriaVirtualLivre", (UInt64)item["FreeVirtualMemory"] / 1024);
            }

            return resultDict;
        }

        private Dictionary<string, object> GetDrivesInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            var resultDict = new Dictionary<string, Object>();
            var list = new List<Object>();

            foreach (var d in allDrives)
            {
                var drive = new
                {
                    NomeDrive = d.Name,
                    TipoDrive = d.DriveType.ToString(),
                    LabelVolume = d.IsReady ? d.VolumeLabel : null,
                    SistemaArquivo = d.IsReady ? d.DriveFormat.ToString() : null,
                    EspacoTotal = d.IsReady ? (d.TotalSize / 1024) / 1024 : 0,
                    EspacoDisponivel = d.IsReady ? (d.AvailableFreeSpace / 1024) / 1024 : 0
                };

                list.Add(drive);
            }
            resultDict.Add("Drives", list);

            return resultDict;
        }

        private Dictionary<string, object> GetNetworkInfo()
        {
            var wql = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
            var searcher = new ManagementObjectSearcher(wql);
            var results = searcher.Get();
            var resultDict = new Dictionary<string, Object>();
            var interfaceList = new List<Object>();

            foreach (var item in results)
            {
                if (Convert.ToInt32(item["NetConnectionStatus"]).Equals(2))
                {
                    var iface = new
                    {
                        NomeInterface = (string)item["Name"],
                        Velocidade = Convert.ToUInt64(item["Speed"]) / 1048576
                    };

                    interfaceList.Add(iface);
                }

            }
            resultDict.Add("Interfaces", interfaceList);
            return resultDict;
        }

        private Dictionary<string, Object> GetProcessorInfo()
        {
            var wql = new ObjectQuery("SELECT * FROM Win32_Processor");
            var searcher = new ManagementObjectSearcher(wql);
            var results = searcher.Get();
            var resultDict = new Dictionary<string, Object>();

            //foreach (var item in results)
            //{
            //    resultDict.Add("PorcentagemUsoProcessor", (UInt16)item["LoadPercentage"]);
            //}

            return resultDict;
        }
    }
}
