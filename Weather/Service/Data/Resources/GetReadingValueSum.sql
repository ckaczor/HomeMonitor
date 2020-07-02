SELECT SUM(@Value)
FROM Reading
WHERE Timestamp BETWEEN @Start AND @End
