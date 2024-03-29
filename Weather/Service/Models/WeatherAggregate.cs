using ChrisKaczor.HomeMonitor.Weather.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models;

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

    private static readonly List<int> WindDirectionValues = ((WindDirection[])Enum.GetValues(typeof(WindDirection))).Select(e => (int)e).ToList();

    public WeatherAggregate(List<WeatherReading> readings)
    {
        if (!readings.Any())
            return;

        Humidity = new ReadingAggregate(readings, r => r.Humidity, 1);

        Temperature = new ReadingAggregate(readings, r => r.HumidityTemperature, 1);

        Pressure = new ReadingAggregate(readings, r => r.Pressure, 2);

        Light = new ReadingAggregate(readings, r => r.LightLevel, 2);

        WindSpeed = new ReadingAggregate(readings, r => r.WindSpeed, 1);

        var windDirectionAverage = readings.Average(r => (decimal)r.WindDirection);
        WindDirectionAverage = (WindDirection)WindDirectionValues.Aggregate((x, y) => Math.Abs(x - windDirectionAverage) < Math.Abs(y - windDirectionAverage) ? x : y);

        RainTotal = readings.Sum(r => r.Rain);
    }
}