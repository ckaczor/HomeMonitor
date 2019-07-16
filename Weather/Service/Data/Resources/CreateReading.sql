INSERT INTO weather_reading (timestamp, wind_direction, wind_speed, humidity, humidity_temperature, rain, pressure,
                             pressure_temperature, battery_level, light_level, latitude, longitude, altitude,
                             satellite_count, gps_timestamp)
VALUES (:timestamp, :windDirection, :windSpeed, :humidity, :humidityTemperature, :rain, :pressure, :pressureTemperature,
        :batteryLevel, :lightLevel, :latitude, :longitude, :altitude, :satelliteCount, :gpsTimestamp)
ON CONFLICT DO NOTHING 