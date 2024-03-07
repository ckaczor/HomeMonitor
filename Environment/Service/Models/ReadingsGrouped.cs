using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Models;

[PublicAPI]
public class ReadingsGrouped
{
    [JsonPropertyName("bucket")]
    public DateTimeOffset Bucket { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("averageTemperature")]
    public decimal AverageTemperature { get; set; }

    [JsonPropertyName("averagePressure")]
    public decimal AveragePressure { get; set; }

    [JsonPropertyName("averageHumidity")]
    public decimal AverageHumidity { get; set; }

    [JsonPropertyName("averageLuminance")]
    public decimal AverageLuminance { get; set; }

    [JsonPropertyName("averageGasResistance")]
    public decimal AverageGasResistance { get; set; }

    [JsonPropertyName("averageColorTemperature")]
    public decimal AverageColorTemperature { get; set; }

    [JsonPropertyName("averageAirQualityIndex")]
    public decimal AverageAirQualityIndex { get; set; }
}