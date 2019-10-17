SELECT Bucket,
	   AVG(Value) AS AverageValue
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
			   @Value AS Value
		FROM Reading
		WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket