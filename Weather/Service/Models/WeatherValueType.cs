﻿using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models;

[PublicAPI]
public enum WeatherValueType
{
    WindSpeed,
    Humidity,
    HumidityTemperature,
    Rain,
    Pressure,
    PressureTemperature,
    LightLevel,
    Altitude,
    SatelliteCount
}