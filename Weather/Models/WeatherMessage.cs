using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
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

            WindDirection = (WindDirection)Enum.Parse(typeof(WindDirection), messageValues[@"winddir"]);
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