using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Models;

public class ReadingsAggregate
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("minimumTemperature")]
    public decimal MinimumTemperature { get; set; }

    [JsonPropertyName("averageTemperature")]
    public decimal AverageTemperature { get; set; }

    [JsonPropertyName("maximumTemperature")]
    public decimal MaximumTemperature { get; set; }

    [JsonPropertyName("minimumPressure")]
    public decimal MinimumPressure { get; set; }

    [JsonPropertyName("averagePressure")]
    public decimal AveragePressure { get; set; }

    [JsonPropertyName("maximumPressure")]
    public decimal MaximumPressure { get; set; }

    [JsonPropertyName("minimumHumidity")]
    public decimal MinimumHumidity { get; set; }

    [JsonPropertyName("averageHumidity")]
    public decimal AverageHumidity { get; set; }

    [JsonPropertyName("maximumHumidity")]
    public decimal MaximumHumidity { get; set; }
}