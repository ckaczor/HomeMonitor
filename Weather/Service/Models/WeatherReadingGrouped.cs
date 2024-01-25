using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models;

[PublicAPI]
public class WeatherReadingGrouped
{
    public DateTimeOffset Bucket { get; set; }

    public decimal AverageHumidity { get; set; }

    public decimal AverageTemperature { get; set; }

    public decimal AveragePressure { get; set; }

    public decimal AverageLightLevel { get; set; }

    public decimal RainTotal { get; set; }
}