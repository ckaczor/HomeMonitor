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
        public MessageType Type { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WindDirection WindDirection { get; private set; }

        public double WindSpeed { get; private set; }

        public double Humidity { get; private set; }

        public double HumidityTemperature { get; private set; }

        public double Rain { get; private set; }

        public double Pressure { get; private set; }

        public double PressureTemperature { get; private set; }

        public double BatteryLevel { get; private set; }

        public double LightLevel { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public double Altitude { get; private set; }

        public int SatelliteCount { get; private set; }

        public DateTimeOffset GpsTimestamp { get; private set; }

        public string Message { get; private set; }

        private WeatherMessage()
        {
            Type = MessageType.Text;
            Timestamp = DateTimeOffset.UtcNow;
        }

        private WeatherMessage(string message)
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