SELECT Bucket,
	   MIN(WindSpeed) AS Minimum,
	   AVG(WindSpeed) AS Average,
	   MAX(WindSpeed) AS Maximum
FROM (
         SELECT CAST(FORMAT(Timestamp, 'yyyy-MM-ddTHH:') +
                RIGHT('00' + CAST(DATEPART(MINUTE, Timestamp) / @BucketMinutes * @BucketMinutes AS VARCHAR), 2)
				+ ':00+00:00' AS DATETIMEOFFSET)									  AS Bucket,
                WindSpeed
         FROM Reading
         WHERE Timestamp BETWEEN @Start AND @End
     ) AS Data
GROUP BY Bucket
ORDER BY Bucket