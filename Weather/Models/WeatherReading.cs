using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    [PublicAPI]
    public class WeatherReading
    {
        public DateTimeOffset Timestamp { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WindDirection WindDirection { get; set; }

        public decimal WindSpeed { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public decimal HumidityTemperature { get; set; }

        public decimal Rain { get; set; }

        public decimal Pressure { get; set; }

        public decimal PressureTemperature { get; set; }

        public decimal LightLevel { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal Altitude { get; set; }

        public int SatelliteCount { get; set; }

        public DateTimeOffset GpsTimestamp { get; set; }
    }
}