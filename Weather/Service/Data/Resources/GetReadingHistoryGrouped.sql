SELECT Bucket,
	   AVG(Humidity) AS AverageHumidity,
       AVG(Temperature) AS AverageTemperature,
       AVG(Pressure) AS AveragePressure,
	   AVG(LightLevel) AS AverageLightLevel,
       SUM(Rain) AS RainTotal
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
				Humidity,
				HumidityTemperature AS Temperature,
				Pressure,
				LightLevel,
                Rain
         FROM Reading
         WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket