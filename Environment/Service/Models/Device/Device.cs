namespace ChrisKaczor.HomeMonitor.Environment.Service.Models.Device;

public class Device
{
    public required string Name { get; set; }
    public DateTimeOffset? LastUpdated { get; set; }
}
