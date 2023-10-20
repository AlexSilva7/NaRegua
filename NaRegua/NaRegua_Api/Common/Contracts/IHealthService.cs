namespace NaRegua_Api.Common.Contracts
{
    public interface IHealthService
    {
        HealthCheckResult GetSystemInfo();
        MemoryInfo GetMemoryInfo();
        CpuInfo GetCpuInfoInfo();
        QueueInfo GetQueueServiceInfo();
        CacheInfo GetCacheServiceInfo();
    }

    public class HealthCheckResult
    {
        public string HostName { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
        public CpuInfo CpuInfo { get; set; }
        public CacheInfo CacheInfo { get; set; }
        public QueueInfo QueueInfo { get; set; }
    }
    public class MemoryInfo
    {
        public string TotalMemory { get; set; }
        public string MemoryUsage { get; set; }
        public string FreeMemory { get; set; }
    }

    public class CpuInfo
    {
        public string CpuUsage { get; set; }
    }

    public class QueueInfo
    {
        public string QueueProvider { get; set; }
        public bool IsConnected { get; set; }
        public int QueueLength { get; set; }
        public QueueLevel QueueLevel { get; set; }
    }

    public class CacheInfo
    {
        public string CacheProvider { get; set; }
        public bool IsConnected { get; set; }
    }

    public enum QueueLevel
    {
        Ok = 0,
        Warning = 500,
        Critical = 1000
    }
}
