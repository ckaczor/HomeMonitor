SELECT Bucket,
	   AVG(Generation) AS AverageGeneration,
	   AVG(Consumption) AS AverageConsumption
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
                Generation,
				Consumption
         FROM Status
         WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket