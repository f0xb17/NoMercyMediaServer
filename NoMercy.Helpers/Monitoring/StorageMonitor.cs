
namespace NoMercy.Helpers.Monitoring
{
    public class StorageMonitor
    {
        public static List<ResourceMonitorDto> Main()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<ResourceMonitorDto> resourceMonitorDtos = new();
            
            foreach (DriveInfo d in allDrives)
            {
                ResourceMonitorDto resourceMonitorDto = new ResourceMonitorDto
                {
                    Name = d.Name,
                    Type = d.DriveType,
                };
                if (d.IsReady == true)
                {
                    resourceMonitorDto.TotalStorage = $"Total size of drive: {d.TotalSize / 1024 / 1024 / 1024} GB";
                    resourceMonitorDto.AvailableStorage =
                        $"Total available space: {d.AvailableFreeSpace / 1024 / 1024 / 1024} GB";
                }
                
                resourceMonitorDtos.Add(resourceMonitorDto);
            }
            
            return resourceMonitorDtos;
        }
    }
}

public class ResourceMonitorDto
{
    public string Name { get; set; }
    public DriveType Type { get; set; }
    public string TotalStorage { get; set; }
    public string AvailableStorage { get; set; }
}