using System;
using System.Collections.Generic;
using System.Linq;
using ChrisKaczor.HomeMonitor.Weather.Models;
using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WeatherAggregate
    {
        public ReadingAggregate Humidity { get; set; }

        public ReadingAggregate Temperature { get; set; }

        public ReadingAggregate Pressure { get; set; }

        public ReadingAggregate Light { get; set; }

        public ReadingAggregate WindSpeed { get; set; }

        public WindDirection WindDirectionAverage { get; set; }

        public decimal RainTotal { get; set; }

        private readonly static List<int> _windDirectionValues = ((WindDirection[])Enum.GetValues(typeof(WindDirection))).Select(e => (int)e).ToList();

        public WeatherAggregate(IEnumerable<WeatherReading> readings)
        {
            if (!readings.Any())
                return;

            Humidity = new ReadingAggregate(readings, r => r.Humidity, 1);

            Temperature = new ReadingAggregate(readings, r => r.PressureTemperature, 1);

            Pressure = new ReadingAggregate(readings, r => r.Pressure, 2);

            Light = new ReadingAggregate(readings, r => (r.LightLevel / 3.3m * 100).Truncate(1), 1);

            WindSpeed = new ReadingAggregate(readings, r => r.WindSpeed, 1);

            var windDirectionAverage = readings.Average(r => (decimal)r.WindDirection);
            WindDirectionAverage = (WindDirection)_windDirectionValues.Aggregate((x, y) => Math.Abs(x - windDirectionAverage) < Math.Abs(y - windDirectionAverage) ? x : y);

            RainTotal = readings.Sum(r => r.Rain);
        }
    }
}