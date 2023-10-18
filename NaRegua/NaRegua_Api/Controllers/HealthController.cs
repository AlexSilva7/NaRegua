using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Management;

namespace NaRegua_Api.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/health")] // GET /health
        public async Task<IActionResult> GetHealthInfoAsync()
        {
            try
            {
                string hostName = System.Net.Dns.GetHostName();

                if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    //MEMORIA FUNCIONANDO
                    //ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                    //ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    //ManagementObject obj = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                    //if (obj != null)
                    //{
                    //    ulong totalMemory = Convert.ToUInt64(obj["TotalVisibleMemorySize"]);
                    //    ulong freeMemory = Convert.ToUInt64(obj["FreePhysicalMemory"]);
                    //    ulong usedMemory = totalMemory - freeMemory;

                    //    var a = $"Total de Memória: {totalMemory / 1024} MB";
                    //    var B = $"Memória Livre: {freeMemory / 1024} MB";
                    //    var C = $"Memória Usada: {usedMemory / 1024} MB";
                    //}

                    // Informações de memória no Windows
                    //var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                    //var query = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
                    //if (query != null)
                    //{
                    //    ulong totalMemory = Convert.ToUInt64(query["TotalPhysicalMemory"]);
                    //    //ulong freeMemory = Convert.ToUInt64(query["FreePhysicalMemory"]);

                    //    Console.WriteLine($"Memória Total: {totalMemory / (1024 * 1024)} MB");

                    //info CPU
                    //ObjectQuery query2 = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'");
                    //ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(query2);
                    //ManagementObjectCollection collection = searcher2.Get();

                    //ManagementObject obj = collection.OfType<ManagementObject>().FirstOrDefault();
                    //if (obj != null)
                    //{
                    //    var o = $"Uso da CPU Total: {obj["PercentProcessorTime"]}%";
                    //}

                    //return Ok(totalMemory / (1024 * 1024));
                    //}
                }
                else if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    // Informações de memória no Linux
                    string memInfoPath = "/proc/meminfo";
                    if (System.IO.File.Exists(memInfoPath))
                    {
                        var lines1 = System.IO.File.ReadLines(memInfoPath).Take(2).ToList();
                        var totalMemory = lines1[0].Split(':');
                        var freeMemory = lines1[1].Split(':');

                        Console.WriteLine($"Memória Total: {ParseMemInfoValue(totalMemory[1]) / 1024} KB");
                        Console.WriteLine($"Memória Livre: {ParseMemInfoValue(freeMemory[1]) / 1024} KB");

                        //return Ok($"Memória Total: {ParseMemInfoValue(totalMemory[1])} MB, Livre: {ParseMemInfoValue(freeMemory[1]) / 1024} MB");

                        //CPU LINUX
                        string cpuInfoPath = "/proc/stat";

                        if (System.IO.File.Exists(cpuInfoPath))
                        {
                            string[] lines = System.IO.File.ReadAllLines(cpuInfoPath);

                            foreach (string line in lines)
                            {
                                if (line.StartsWith("cpu "))
                                {
                                    string[] values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (values.Length >= 9)
                                    {
                                        // Observe que os valores estão em unidades de tempo em milissegundos.
                                        ulong user = ulong.Parse(values[1]);
                                        ulong nice = ulong.Parse(values[2]);
                                        ulong system = ulong.Parse(values[3]);
                                        ulong idle = ulong.Parse(values[4]);
                                        ulong iowait = ulong.Parse(values[5]);
                                        ulong irq = ulong.Parse(values[6]);
                                        ulong softirq = ulong.Parse(values[7]);
                                        ulong steal = ulong.Parse(values[8]);

                                        ulong totalCpuTime = user + nice + system + idle + iowait + irq + softirq + steal;

                                        ulong idleCpuTime = idle;
                                        double cpuUsage = 100.0 - ((idleCpuTime / (double)totalCpuTime) * 100.0);

                                        Console.WriteLine($"Porcentagem de Uso de CPU: {Math.Round(cpuUsage, 2)}%");

                                        return Ok($"Porcentagem de Uso de CPU: {Math.Round(cpuUsage, 2)}%");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Informações de memória não disponíveis para este sistema operacional.");
                }

                return Ok(hostName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        static ulong ParseMemInfoValue(string line)
        {
            var value = line.Trim().Split(' ')[0];
            return ulong.Parse(value);
        }
    }
}
