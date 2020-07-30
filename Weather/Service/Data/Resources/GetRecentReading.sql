SELECT TOP 1 Timestamp,
             WindDirection,
             WindSpeed,
             Humidity,
             Rain,
             Pressure,
             PressureTemperature AS Temperature,
             BatteryLevel,
             LightLevel,
             Latitude,
             Longitude,
             Altitude,
             SatelliteCount,
             GpsTimestamp
FROM Reading
ORDER BY Timestamp DESC