CREATE TABLE
    reading (
        time timestamptz NOT NULL,
        name text NOT NULL,
        model text NOT NULL,
        temperature DECIMAL NOT NULL,
        pressure DECIMAL NOT NULL,
        humidity DECIMAL NOT NULL,
        luminance INT NOT NULL,
        gas_resistance INT NOT NULL,
        color_temperature INT NOT NULL,
        air_quality_index DECIMAL NOT NULL,
        CONSTRAINT reading_pk PRIMARY KEY (time, name, model)
    );

SELECT
    create_hypertable('reading', by_range('time'));