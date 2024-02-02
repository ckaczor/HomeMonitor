using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Models;

[PublicAPI]
public class ReadingsGrouped
{
    public DateTimeOffset Bucket { get; set; }

    public string? Name { get; set; }

    public decimal AverageTemperature { get; set; }

    public decimal AveragePressure { get; set; }

    public decimal AverageHumidity { get; set; }

    public decimal AverageLuminance { get; set; }

    public decimal AverageGasResistance { get; set; }

    public decimal AverageColorTemperature { get; set; }

    public decimal AverageAirQualityIndex { get; set; }
}