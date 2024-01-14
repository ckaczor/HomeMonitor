using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

[UsedImplicitly]
public class Readings
{
    [JsonPropertyName("aqi")]
    public decimal AirQualityIndex { get; set; }

    [JsonPropertyName("color_temperature")]
    public decimal ColorTemperature { get; set; }

    [JsonPropertyName("gas_resistance")]
    public decimal GasResistance { get; set; }

    [JsonPropertyName("humidity")]
    public decimal Humidity { get; set; }

    [JsonPropertyName("luminance")]
    public decimal Luminance { get; set; }

    [JsonPropertyName("pressure")]
    public decimal Pressure { get; set; }

    [JsonPropertyName("temperature")]
    public decimal Temperature { get; set; }
}