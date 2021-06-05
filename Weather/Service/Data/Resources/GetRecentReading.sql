SELECT TOP 1 Timestamp,
             WindDirection,
             WindSpeed,
             Humidity,
             Rain,
             Pressure,
             HumidityTemperature AS Temperature,
             LightLevel,
             Latitude,
             Longitude,
             Altitude,
             SatelliteCount,
             GpsTimestamp
FROM Reading
ORDER BY Timestamp DESC