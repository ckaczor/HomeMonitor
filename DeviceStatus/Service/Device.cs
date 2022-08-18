using System.Text.Json.Serialization;

namespace Service;

public class Device
{
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("status")]
    public bool Status { get; private set; }

    public Device(string name, string statusString)
    {
        Name = name;
        Update(statusString);
    }

    public void Update(string statusString)
    {
        Status = statusString == "1";
    }
}