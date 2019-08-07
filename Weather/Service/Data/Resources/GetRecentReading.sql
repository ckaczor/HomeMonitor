SELECT TOP 1 Timestamp,
             WindDirection,
             WindSpeed,
             Humidity,
             HumidityTemperature,
             Rain,
             Pressure,
             PressureTemperature,
             BatteryLevel,
             LightLevel,
             Latitude,
             Longitude,
             Altitude,
             SatelliteCount,
             GpsTimestamp
FROM Reading
ORDER BY Timestamp DESC