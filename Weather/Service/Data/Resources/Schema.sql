IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'Reading')
	CREATE TABLE Reading
	(
		Timestamp           datetimeoffset NOT NULL
			CONSTRAINT reading_pk
				PRIMARY KEY,
		WindDirection       int            NOT NULL,
		WindSpeed           decimal(4, 1)  NOT NULL,
		Humidity            decimal(4, 1)  NOT NULL,
		HumidityTemperature decimal(4, 1)  NOT NULL,
		Rain                decimal(2, 2)  NOT NULL,
		Pressure            decimal(8, 2)  NOT NULL,
		PressureTemperature decimal(4, 1)  NOT NULL,
		BatteryLevel        decimal(3, 2)  NOT NULL,
		LightLevel          decimal(3, 2)  NOT NULL,
		Latitude            decimal(9, 6)  NOT NULL,
		Longitude           decimal(9, 6)  NOT NULL,
		Altitude            decimal(5, 1)  NOT NULL,
		SatelliteCount      int            NOT NULL,
		GpsTimestamp        datetimeoffset NOT NULL
	);
