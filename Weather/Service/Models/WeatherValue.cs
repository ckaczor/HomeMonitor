using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WeatherValue
    {
        public DateTimeOffset Timestamp { get; set; }

        public decimal Value { get; set; }
    }
}
