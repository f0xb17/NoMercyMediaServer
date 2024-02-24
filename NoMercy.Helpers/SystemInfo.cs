using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using DeviceId;

namespace NoMercy.Helpers;

public static class SystemInfo
{
    public static readonly string DeviceName = "VLIEGTUIG!";
    public static readonly string DeviceId = GetDeviceId();
    public static readonly string Os = RuntimeInformation.OSDescription;
    public static readonly string Platform = GetPlatform();
    public static readonly string Architecture = RuntimeInformation.ProcessArchitecture.ToString();
    public static readonly string? Cpu = GetCpuFullName();
    public static readonly string? Version = GetSystemVersion();
    public static readonly DateTime BootTime = GetBootTime();
    public static readonly DateTime StartTime = DateTime.UtcNow;
    public static readonly string ExecSuffix = Platform == "windows" ? ".exe" : "";
    public static int ServerPort { get; set; } = 7626;
    
    private static string GetDeviceId()
    {
        string generatedId = new DeviceIdBuilder()
            .AddMachineName()
            .AddOsVersion()
            .OnWindows(windows => windows
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnLinux(linux => linux
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnMac(mac => mac
                .AddSystemDriveSerialNumber()
                .AddPlatformSerialNumber())
            .ToString();

        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(generatedId));

        return new Guid(hash).ToString();
    }
    private static string GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "windows";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "mac";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "linux";
        
        throw new Exception("Unknown platform");
    }

    private static string? GetCpuFullName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_Processor");
            foreach (var o in searcher.Get())
            {
                var item = (ManagementObject)o;
                return item["Name"].ToString()?.Trim();
            }
        }
        else
        {
            var output = ExecuteBashCommand("lscpu | grep 'Model name:'");
            return output.Trim().Split(':')[1].Trim();
        }
        return "Unknown";
    }

    private static string? GetSystemVersion()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Version from Win32_OperatingSystem");
            foreach (var o in searcher.Get())
            {
                var item = (ManagementObject)o;
                return item["Version"].ToString();
            }
        }
        else
        {
            var output = ExecuteBashCommand("uname -r");
            return output.Trim();
        }
        return "Unknown";
    }

    private static DateTime GetBootTime()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select LastBootUpTime from Win32_OperatingSystem");
            foreach (var o in searcher.Get())
            {
                var item = (ManagementObject)o;
                return ManagementDateTimeConverter.ToDateTime(item["LastBootUpTime"].ToString());
            }
        }
        else
        {
            var output = ExecuteBashCommand("uptime -s");
            return DateTime.Parse(output.Trim());
        }
        return DateTime.MinValue;
    }

    private static string ExecuteBashCommand(string command)
    {
        command = command.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }
}