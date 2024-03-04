using ChrisKaczor.HomeMonitor.Environment.Service.Models.Indoor;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Models;

public class Readings
{
    public Readings()
    {
    }

    public Readings(DeviceMessage message)
    {
        Time = message.Timestamp;
        Name = message.Name;
        Model = message.Model;
        AirQualityIndex = message.AirQualityIndex;
        ColorTemperature = message.ColorTemperature;
        GasResistance = message.GasResistance;
        Humidity = message.Humidity;
        Luminance = message.Luminance;
        Pressure = message.Pressure;
        Temperature = message.Temperature;
    }

    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("airQualityIndex")]
    public decimal AirQualityIndex { get; set; }

    [JsonPropertyName("colorTemperature")]
    public decimal ColorTemperature { get; set; }

    [JsonPropertyName("gasResistance")]
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