SELECT DISTINCT ON (name)
    time,
    name,
    model,
    temperature,
    pressure,
    humidity,
    luminance,
    gas_resistance AS gasResistance,
    color_temperature AS colorTemperature,
    air_quality_index AS airQualityIndex
FROM
    reading
ORDER BY
    name,
    time DESC
