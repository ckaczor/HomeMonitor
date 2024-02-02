CREATE EXTENSION IF NOT EXISTS timescaledb;

CREATE TABLE reading(
  time timestamptz NOT NULL,
  name text NOT NULL,
  model text NOT NULL,
  temperature DECIMAL NOT NULL,
  pressure DECIMAL NOT NULL,
  humidity DECIMAL NOT NULL,
  luminance int NOT NULL,
  gas_resistance int NOT NULL,
  color_temperature int NOT NULL,
  air_quality_index DECIMAL NOT NULL,
  CONSTRAINT reading_pk PRIMARY KEY (time, name, model)
);

SELECT
  create_hypertable('reading', by_range('time'));

ALTER TABLE reading SET (
  timescaledb.compress, 
  timescaledb.compress_segmentby = 'name, model', 
  timescaledb.compress_orderby = 'time DESC');

SELECT
  add_compression_policy('reading', INTERVAL '7 days');
