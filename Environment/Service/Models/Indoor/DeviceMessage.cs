using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Models.Indoor;

public class DeviceMessage
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("nickname")]
    public required string Name { get; set; }

    [JsonPropertyName("readings")]
    public required DeviceReadings Readings { get; set; }

    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("air_quality_index")]
    public decimal AirQualityIndex => Readings.AirQualityIndex;

    [JsonPropertyName("color_temperature")]
    public decimal ColorTemperature => Readings.ColorTemperature;

    [JsonPropertyName("gas_resistance")]
    public decimal GasResistance => Readings.GasResistance;

    [JsonPropertyName("humidity")]
    public decimal Humidity => Readings.Humidity;

    [JsonPropertyName("luminance")]
    public decimal Luminance => Readings.Luminance;

    [JsonPropertyName("pressure")]
    public decimal Pressure => Readings.Pressure;

    [JsonPropertyName("temperature")]
    public decimal Temperature => Readings.Temperature;
}