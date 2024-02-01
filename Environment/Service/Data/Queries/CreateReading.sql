INSERT INTO 
    reading 
    (
        time, 
        name, 
        model, 
        temperature, 
        pressure, 
        humidity, 
        luminance, 
        gas_resistance, 
        color_temperature, 
        air_quality_index
    )
VALUES 
    (
        @Timestamp, 
        @Name, 
        @Model, 
        @Temperature, 
        @Pressure, 
        @Humidity, 
        @Luminance, 
        @GasResistance, 
        @ColorTemperature, 
        @AirQualityIndex
    ) 
ON CONFLICT 
    ON CONSTRAINT reading_pk 
        DO NOTHING