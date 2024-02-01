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

    public decimal AirQualityIndex => Readings.AirQualityIndex;

    public decimal ColorTemperature => Readings.ColorTemperature;

    public decimal GasResistance => Readings.GasResistance;

    public decimal Humidity => Readings.Humidity;

    public decimal Luminance => Readings.Luminance;

    public decimal Pressure => Readings.Pressure;

    public decimal Temperature => Readings.Temperature;
}