SELECT Timestamp,
       @Value AS Value
FROM Reading
WHERE Timestamp BETWEEN @Start AND @End
