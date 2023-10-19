namespace NaRegua_Api.Common.Contracts
{
    public interface IHealthService
    {
        HealthCheckResult GetSystemInfo();
        MemoryInfo GetMemoryInfo();
        CpuInfo GetCpuInfoInfo();
    }

    public class HealthCheckResult
    {
        public string HostName { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
        public CpuInfo CpuInfo { get; set; }

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
}
