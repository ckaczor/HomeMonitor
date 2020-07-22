using System.Linq;
using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using DecimalMath;
using JetBrains.Annotations;
using MathNet.Numerics;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WeatherUpdate : WeatherMessage
    {
        private readonly Database _database;

        public decimal? WindChill { get; set; }

        public decimal? HeatIndex { get; set; }

        public decimal DewPoint { get; set; }

        public decimal PressureDifferenceThreeHour { get; set; }

        public decimal RainLastHour { get; set; }

        public WeatherUpdate(WeatherMessage weatherMessage, Database database)
        {
            _database = database;

            Type = weatherMessage.Type;
            Message = weatherMessage.Message;
            Timestamp = weatherMessage.Timestamp;
            WindDirection = weatherMessage.WindDirection;
            WindSpeed = weatherMessage.WindSpeed;
            Humidity = weatherMessage.Humidity;
            HumidityTemperature = weatherMessage.HumidityTemperature;
            Rain = weatherMessage.Rain;
            Pressure = weatherMessage.Pressure;
            PressureTemperature = weatherMessage.PressureTemperature;
            BatteryLevel = weatherMessage.BatteryLevel;
            LightLevel = weatherMessage.LightLevel;
            Latitude = weatherMessage.Latitude;
            Longitude = weatherMessage.Longitude;
            Altitude = weatherMessage.Altitude;
            SatelliteCount = weatherMessage.SatelliteCount;
            GpsTimestamp = weatherMessage.GpsTimestamp;

            CalculateHeatIndex();
            CalculateWindChill();
            CalculateDewPoint();
            CalculatePressureTrend();
            CalculateRainLastHour();
        }

        private void CalculateRainLastHour()
        {
            RainLastHour = _database.GetReadingValueSum(WeatherValueType.Rain, Timestamp.AddHours(-1), Timestamp).Result;
        }

        private void CalculatePressureTrend()
        {
            var pressureData = _database.GetReadingValueHistory(WeatherValueType.Pressure, Timestamp.AddHours(-3), Timestamp).Result;

            var xData = pressureData.Select(p => (double)p.Timestamp.ToUnixTimeSeconds()).ToArray();
            var yData = pressureData.Select(p => (double)p.Value / 100.0).ToArray();

            var lineFunction = Fit.LineFunc(xData, yData);

            PressureDifferenceThreeHour = (decimal)(lineFunction(xData.Last()) - lineFunction(xData[0]));
        }

        private void CalculateHeatIndex()
        {
            var temperature = PressureTemperature;
            var humidity = Humidity;

            if (temperature.IsBetween(80, 100) && humidity.IsBetween(40, 100))
            {
                HeatIndex = -42.379m + 2.04901523m * temperature + 10.14333127m * humidity - .22475541m * temperature * humidity - .00683783m * temperature * temperature -
                            .05481717m * humidity * humidity + .00122874m * temperature * temperature * humidity + .00085282m * temperature * humidity * humidity -
                            .00000199m * temperature * temperature * humidity * humidity;
            }
        }

        private void CalculateWindChill()
        {
            var temperatureInF = PressureTemperature;
            var windSpeedInMph = WindSpeed;

            if (temperatureInF.IsBetween(-45, 45) && windSpeedInMph.IsBetween(3, 60))
            {
                WindChill = 35.74m + 0.6215m * temperatureInF - 35.75m * DecimalEx.Pow(windSpeedInMph, 0.16m) + 0.4275m * temperatureInF * DecimalEx.Pow(windSpeedInMph, 0.16m);
            }
        }

        private void CalculateDewPoint()
        {
            var relativeHumidity = Humidity;
            var temperatureInF = PressureTemperature;

            var temperatureInC = (temperatureInF - 32.0m) * 5.0m / 9.0m;

            var vaporPressure = relativeHumidity * 0.01m * 6.112m * DecimalEx.Exp(17.62m * temperatureInC / (temperatureInC + 243.12m));
            var numerator = 243.12m * DecimalEx.Log(vaporPressure) - 440.1m;
            var denominator = 19.43m - DecimalEx.Log(vaporPressure);
            var dewPointInC = numerator / denominator;

            DewPoint = dewPointInC * 9.0m / 5.0m + 32.0m;
        }
    }
}
