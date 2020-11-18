using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    [PublicAPI]
    public class WeatherUpdateBase
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }

        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WindDirection WindDirection { get; set; }

        public decimal WindSpeed { get; set; }
        public decimal Humidity { get; set; }
        public decimal Rain { get; set; }
        public decimal Pressure { get; set; }
        public decimal Temperature { get; set; }
        public decimal LightLevel { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Altitude { get; set; }
        public int SatelliteCount { get; set; }
        public DateTimeOffset GpsTimestamp { get; set; }
        public decimal? WindChill { get; set; }
        public decimal? HeatIndex { get; set; }
        public decimal DewPoint { get; set; }
        public decimal PressureDifferenceThreeHour { get; set; }
        public decimal RainLastHour { get; set; }
    }
}