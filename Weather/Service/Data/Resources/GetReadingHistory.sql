SELECT Timestamp,
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
WHERE Timestamp BETWEEN @Start AND @End
ORDER BY Timestamp ASC