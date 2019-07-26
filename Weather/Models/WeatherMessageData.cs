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

        public double WindSpeed { get; set; }

        public double Humidity { get; set; }

        public double HumidityTemperature { get; set; }

        public double Rain { get; set; }

        public double Pressure { get; set; }

        public double PressureTemperature { get; set; }

        public double BatteryLevel { get; set; }

        public double LightLevel { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

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
            WindSpeed = double.Parse(messageValues[@"windspeedmph"]);
            Humidity = double.Parse(messageValues[@"humidity"]);
            HumidityTemperature = double.Parse(messageValues[@"tempH"]);
            Rain = double.Parse(messageValues[@"rain"]);
            Pressure = double.Parse(messageValues[@"pressure"]);
            PressureTemperature = double.Parse(messageValues[@"tempP"]);
            BatteryLevel = double.Parse(messageValues[@"batt_lvl"]);
            LightLevel = double.Parse(messageValues[@"light_lvl"]);
            Latitude = double.Parse(messageValues[@"lat"]);
            Longitude = double.Parse(messageValues[@"lng"]);
            Altitude = double.Parse(messageValues[@"altitude"]);
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