IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'Status')
	CREATE TABLE Status
	(
		Timestamp           datetimeoffset	NOT NULL
			CONSTRAINT status_pk
				PRIMARY KEY,
		Generation          int				NOT NULL,
		Consumption			int				NOT NULL
	);
