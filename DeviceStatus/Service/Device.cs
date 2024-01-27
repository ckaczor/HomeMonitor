using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.DeviceStatus.Service;

public class Device
{
    public Device(string name, string statusString)
    {
        Name = name;
        Update(statusString);
    }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("status")]
    public bool Status { get; private set; }

    private void Update(string statusString)
    {
        Status = statusString == "1";
    }
}