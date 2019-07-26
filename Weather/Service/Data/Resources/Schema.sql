CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;

CREATE TABLE IF NOT EXISTS weather_reading
(
  timestamp            timestamptz      NOT NULL
    CONSTRAINT weather_reading_pk
      PRIMARY KEY,
  wind_direction       int              NOT NULL,
  wind_speed           double precision NOT NULL,
  humidity             double precision NOT NULL,
  humidity_temperature double precision NOT NULL,
  rain                 double precision NOT NULL,
  pressure             double precision NOT NULL,
  pressure_temperature double precision NOT NULL,
  battery_level        double precision NOT NULL,
  light_level          double precision NOT NULL,
  latitude             double precision NOT NULL,
  longitude            double precision NOT NULL,
  altitude             double precision NOT NULL,
  satellite_count      double precision NOT NULL,
  gps_timestamp        timestamptz      NOT NULL
);

SELECT create_hypertable('weather_reading', 'timestamp', if_not_exists => TRUE);