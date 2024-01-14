BEGIN TRANSACTION

INSERT Reading
  (Timestamp, Name, Model, Temperature, Pressure, Humidity, Luminance, GasResistance, ColorTemperature, AirQualityIndex)
SELECT
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
WHERE NOT EXISTS
  (
    SELECT
      1
    FROM
      Reading WITH (UPDLOCK, SERIALIZABLE)
    WHERE Timestamp = @Timestamp AND Name = @Name AND Model = @Model
  )

COMMIT TRANSACTION