CREATE TABLE Reading
(
  Timestamp        datetimeoffset NOT NULL,
  Name             nvarchar(50)   NOT NULL,
  Model            nvarchar(50)   NOT NULL,
  Temperature      decimal(5, 2)  NOT NULL,
  Pressure         decimal(6, 2)  NOT NULL,
  Humidity         decimal(5, 2)  NOT NULL,
  Luminance        int            NOT NULL,
  GasResistance    int            NOT NULL,
  ColorTemperature int            NOT NULL,
  AirQualityIndex  decimal(4, 1)  NOT NULL,
  CONSTRAINT reading_pk PRIMARY KEY (Timestamp, Name, Model)
);
