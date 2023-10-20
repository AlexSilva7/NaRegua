using NaRegua_Api.Common.Contracts;
using System.Management;

namespace NaRegua_Api.Providers
{
    public class HealthService : IHealthService
    {
        private readonly ICacheService _cacheService;
        private readonly IQueueService _queueService;
        public HealthService(ICacheService cacheService, IQueueService queueService)
        {
            _cacheService = cacheService;
            _queueService = queueService;
        }
        public CpuInfo GetCpuInfoInfo()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var queryOperatingSystem = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name = '_Total'");
                var searcher = new ManagementObjectSearcher(queryOperatingSystem);
                var collection = searcher.Get();

                var obj = collection.OfType<ManagementObject>().FirstOrDefault();

                if (obj != null)
                {
                    return new CpuInfo
                    {
                        CpuUsage = $"{obj["PercentProcessorTime"]}%"
                    };
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                string cpuInfoPath = "/proc/stat";

                if (File.Exists(cpuInfoPath))
                {
                    var lines = File.ReadAllLines(cpuInfoPath);

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("cpu "))
                        {
                            var values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (values.Length >= 9)
                            {
                                // Os valores estão em unidades de tempo em milissegundos.
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

                                return new CpuInfo
                                {
                                    CpuUsage = $"{Math.Round(cpuUsage, 2)}%%"
                                };
                            }
                        }
                    }
                }
            }

            return new CpuInfo();
        }

        public MemoryInfo GetMemoryInfo()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var queryOperatingSystem = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(queryOperatingSystem);
                var obj = searcher.Get().Cast<ManagementObject>().FirstOrDefault();

                if (obj != null)
                {
                    var totalMemory = Convert.ToUInt64(obj["TotalVisibleMemorySize"]);
                    var freeMemory = Convert.ToUInt64(obj["FreePhysicalMemory"]);
                    var usedMemory = totalMemory - freeMemory;

                    return new MemoryInfo
                    {
                        TotalMemory = $"{totalMemory / 1024} MB",
                        MemoryUsage = $"{usedMemory / 1024} MB",
                        FreeMemory = $"{freeMemory / 1024} MB",
                    };
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var memInfoPath = "/proc/meminfo";
                if (File.Exists(memInfoPath))
                {
                    var lines = File.ReadLines(memInfoPath).Take(2).ToList();
                    var totalMemory = ParseMemInfoValue(lines[0].Split(':')[1]);
                    var freeMemory = ParseMemInfoValue(lines[1].Split(':')[1]);
                    var usedMemory = totalMemory - freeMemory;

                    return new MemoryInfo
                    {
                        TotalMemory = $"{totalMemory / 1024} MB",
                        MemoryUsage = $"{usedMemory / 1024} MB",
                        FreeMemory = $"{freeMemory / 1024} MB",
                    };
                }
            }

            return new MemoryInfo();
        }

        public HealthCheckResult GetSystemInfo()
        {
            return new HealthCheckResult
            {
                HostName = System.Net.Dns.GetHostName(),
                MemoryInfo = GetMemoryInfo(),
                CpuInfo = GetCpuInfoInfo(),
                CacheInfo = GetCacheServiceInfo(),
                QueueInfo = GetQueueServiceInfo()
            };
        }

        static ulong ParseMemInfoValue(string line)
        {
            var value = line.Trim().Split(' ')[0];
            return ulong.Parse(value);
        }

        public QueueInfo GetQueueServiceInfo()
        {
            var connectInfo = _queueService.GetConnectionInfo();
            return connectInfo;
        }

        public CacheInfo GetCacheServiceInfo()
        {
            var connectInfo = _cacheService.GetConnectionInfo();
            return connectInfo;
        }
    }
}
