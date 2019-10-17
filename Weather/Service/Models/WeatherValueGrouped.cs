using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WeatherValueGrouped
    {
        public DateTimeOffset Bucket { get; set; }

        public decimal AverageValue { get; set; }
    }
}