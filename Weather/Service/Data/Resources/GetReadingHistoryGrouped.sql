SELECT Bucket,
	   AVG(HumidityTemperature) AS AverageHumidityTemperature,
	   AVG(Humidity) AS AverageHumidity,
       AVG(PressureTemperature) AS AveragePressureTemperature,
       AVG(Pressure) AS AveragePressure,
	   AVG(LightLevel) AS AverageLightLevel,
       SUM(Rain) AS RainTotal
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
                HumidityTemperature,
				Humidity,
				PressureTemperature,
				Pressure,
				LightLevel / BatteryLevel AS LightLevel,
                Rain
         FROM Reading
         WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket