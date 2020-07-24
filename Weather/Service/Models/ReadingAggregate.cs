using System;
using System.Collections.Generic;
using System.Linq;
using ChrisKaczor.HomeMonitor.Weather.Models;
using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class ReadingAggregate
    {
        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public decimal Average { get; set; }

        public ReadingAggregate(IEnumerable<WeatherReading> readings, Func<WeatherReading, decimal> selector, int decimalPlaces)
        {
            Min = readings.Min(selector);
            Max = readings.Max(selector);
            Average = readings.Average(selector).Truncate(decimalPlaces);
        }
    }
}