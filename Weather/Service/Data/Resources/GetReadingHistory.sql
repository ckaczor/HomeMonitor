SELECT Timestamp,
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
WHERE Timestamp BETWEEN @Start AND @End
ORDER BY Timestamp ASC