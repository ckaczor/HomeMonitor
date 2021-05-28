using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;

namespace ChrisKaczor.HomeMonitor.Weather.Models
{
    [PublicAPI]
    public class WeatherMessage : WeatherReading
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }

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

            WindDirection = GetWindDirection(double.Parse(messageValues[@"wd"]));
            WindSpeed = decimal.Parse(messageValues[@"ws"]);
            Humidity = decimal.Parse(messageValues[@"sh"]);
            HumidityTemperature = decimal.Parse(messageValues[@"st"]);
            Rain = decimal.Parse(messageValues[@"r"]);
            Pressure = decimal.Parse(messageValues[@"bp"]);
            PressureTemperature = decimal.Parse(messageValues[@"bt"]);
            LightLevel = decimal.Parse(messageValues[@"tl"]);
            Latitude = decimal.Parse(messageValues[@"glt"]);
            Longitude = decimal.Parse(messageValues[@"gln"]);

            var gpsFix = int.Parse(messageValues[@"gf"]);

            if (gpsFix == 0)
                return;

            Altitude = decimal.Parse(messageValues[@"ga"]);
            SatelliteCount = int.Parse(messageValues[@"gs"]);

            var year = int.Parse(messageValues[@"gdy"]) + 2000;
            var month = int.Parse(messageValues[@"gdm"]);
            var day = int.Parse(messageValues[@"gdd"]);

            var hour = int.Parse(messageValues[@"gth"]);
            var minute = int.Parse(messageValues[@"gtm"]);
            var second = int.Parse(messageValues[@"gts"]);

            GpsTimestamp = new DateTimeOffset(year, month, day, hour, minute, second, 0, TimeSpan.Zero);
        }

        [PublicAPI]
        public static WeatherMessage Parse(string message)
        {
            if (message.StartsWith("$") && message.EndsWith("#"))
                return new WeatherMessage(message);

            return new WeatherMessage { Message = message };
        }

        private static WindDirection GetWindDirection(double degrees)
        {
            switch (degrees)
            {
                case 0:
                    return WindDirection.North;

                case 22.5:
                    return WindDirection.NorthNorthEast;

                case 45:
                    return WindDirection.NorthEast;

                case 67.5:
                    return WindDirection.EastNorthEast;

                case 90:
                    return WindDirection.East;

                case 112.5:
                    return WindDirection.EastSouthEast;

                case 135:
                    return WindDirection.SouthEast;

                case 157.5:
                    return WindDirection.SouthSouthEast;

                case 180:
                    return WindDirection.South;

                case 202.5:
                    return WindDirection.SouthSouthWest;

                case 225:
                    return WindDirection.SouthWest;

                case 247.5:
                    return WindDirection.WestSouthWest;

                case 270:
                    return WindDirection.West;

                case 292.5:
                    return WindDirection.WestNorthWest;

                case 315:
                    return WindDirection.NorthWest;

                case 337.5:
                    return WindDirection.NorthNorthWest;
            }

            return WindDirection.None;
        }
    }
}