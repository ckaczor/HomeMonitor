using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Linq;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    public enum MessageType
    {
        Text,
        Data
    }

    [PublicAPI]
    public enum WindDirection
    {
        None = -1,
        North = 0,
        East = 90,
        South = 180,
        West = 270,
        NorthEast = 45,
        SouthEast = 135,
        SouthWest = 225,
        NorthWest = 315,
        NorthNorthEast = 23,
        EastNorthEast = 68,
        EastSouthEast = 113,
        SouthSouthEast = 158,
        SouthSouthWest = 203,
        WestSouthWest = 248,
        WestNorthWest = 293,
        NorthNorthWest = 338
    }

    [PublicAPI]
    public class WeatherMessage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WindDirection WindDirection { get; set; }

        public decimal WindSpeed { get; set; }

        public decimal Humidity { get; set; }

        public decimal HumidityTemperature { get; set; }

        public decimal Rain { get; set; }

        public decimal Pressure { get; set; }

        public decimal PressureTemperature { get; set; }

        public decimal BatteryLevel { get; set; }

        public decimal LightLevel { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal Altitude { get; set; }

        public int SatelliteCount { get; set; }

        public DateTimeOffset GpsTimestamp { get; set; }

        public string Message { get; set; }

        public WeatherMessage()
        {
            Type = MessageType.Text;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public WeatherMessage(string message)
        {
            Type = MessageType.Data;
            Timestamp = DateTimeOffset.UtcNow;

            var messageParts = message.Split(',').ToList();

            messageParts.RemoveAt(0);
            messageParts.RemoveAt(messageParts.Count - 1);

            var messageValues = messageParts.Select(m => m.Split('=')).ToDictionary(a => a[0], a => a[1]);

            WindDirection = Enum.Parse<WindDirection>(messageValues[@"winddir"]);
            WindSpeed = decimal.Parse(messageValues[@"windspeedmph"]);
            Humidity = decimal.Parse(messageValues[@"humidity"]);
            HumidityTemperature = decimal.Parse(messageValues[@"tempH"]);
            Rain = decimal.Parse(messageValues[@"rain"]);
            Pressure = decimal.Parse(messageValues[@"pressure"]);
            PressureTemperature = decimal.Parse(messageValues[@"tempP"]);
            BatteryLevel = decimal.Parse(messageValues[@"batt_lvl"]);
            LightLevel = decimal.Parse(messageValues[@"light_lvl"]);
            Latitude = decimal.Parse(messageValues[@"lat"]);
            Longitude = decimal.Parse(messageValues[@"lng"]);
            Altitude = decimal.Parse(messageValues[@"altitude"]);
            SatelliteCount = int.Parse(messageValues[@"sats"]);

            DateTimeOffset.TryParseExact($"{messageValues[@"date"]} {messageValues[@"time"]}", "MM/dd/yyyy HH:mm:ss", null, DateTimeStyles.None, out var gpsTimestamp);
            GpsTimestamp = gpsTimestamp;
        }

        [PublicAPI]
        public static WeatherMessage Parse(string message)
        {
            if (message.StartsWith("$") && message.EndsWith("#"))
                return new WeatherMessage(message);

            return new WeatherMessage { Message = message };
        }
    }
}