namespace ChrisKaczor.HomeMonitor.Environment.Service.Models;

public class Readings
{
    public DateTimeOffset Time { get; set; }

    public string? Name { get; set; }

    public string? Model { get; set; }

    public decimal AirQualityIndex { get; set; }

    public decimal ColorTemperature { get; set; }

    public decimal GasResistance { get; set; }

    public decimal Humidity { get; set; }

    public decimal Luminance { get; set; }

    public decimal Pressure { get; set; }

    public decimal Temperature { get; set; }
}