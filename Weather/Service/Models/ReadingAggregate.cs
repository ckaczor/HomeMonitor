using ChrisKaczor.HomeMonitor.Weather.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models;

[PublicAPI]
public class ReadingAggregate(IEnumerable<WeatherReading> readings, Func<WeatherReading, decimal> selector, int decimalPlaces)
{
    public decimal Min { get; set; } = readings.Min(selector);

    public decimal Max { get; set; } = readings.Max(selector);

    public decimal Average { get; set; } = readings.Average(selector).Truncate(decimalPlaces);
}