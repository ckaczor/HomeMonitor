INSERT INTO Reading (Timestamp, WindDirection, WindSpeed, Humidity, HumidityTemperature, Rain, Pressure,
                     PressureTemperature, LightLevel, Latitude, Longitude, Altitude,
                     SatelliteCount, GpsTimestamp)
VALUES (@Timestamp, @WindDirection, @WindSpeed, @Humidity, @HumidityTemperature, @Rain, @Pressure, @PressureTemperature,
        @LightLevel, @Latitude, @Longitude, @Altitude, @SatelliteCount, @GpsTimestamp)