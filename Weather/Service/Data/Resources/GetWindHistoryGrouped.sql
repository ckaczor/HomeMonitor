SELECT Bucket,
	   MIN(WindSpeed) AS MinimumSpeed,
	   AVG(WindSpeed) AS AverageSpeed,
	   MAX(WindSpeed) AS MaximumSpeed,
	   AVG(WindDirection) AS AverageDirection
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
                WindSpeed,
				WindDirection
         FROM Reading
         WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket