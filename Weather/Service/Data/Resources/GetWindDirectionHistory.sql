SELECT WindDirection, COUNT(WindDirection) AS Count
FROM Reading
WHERE Timestamp BETWEEN @Start AND @End
  AND WindDirection != -1
GROUP BY WindDirection