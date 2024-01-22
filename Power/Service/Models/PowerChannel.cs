using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Power.Service.Models;

[PublicAPI]
public class PowerChannel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("ch")]
    public long ChannelNumber { get; set; }

    [JsonPropertyName("eImp_Ws")]
    public long ImportedEnergy { get; set; }

    [JsonPropertyName("eExp_Ws")]
    public long ExportedEnergy { get; set; }

    [JsonPropertyName("p_W")]
    public long RealPower { get; set; }

    [JsonPropertyName("q_VAR")]
    public long ReactivePower { get; set; }

    [JsonPropertyName("v_V")]
    public double Voltage { get; set; }
}