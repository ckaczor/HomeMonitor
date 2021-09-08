package com.chriskaczor.homemonitor.wear.weather

import com.beust.klaxon.Json
import com.beust.klaxon.Klaxon
import java.net.URL
import java.sql.Timestamp

data class WeatherStatus(
    @Json(name = "humidity")
    val humidity: Double,

    @Json(name = "pressure")
    val pressure: Double,
)

object WeatherRepository {
    suspend fun getWeatherStatus(): WeatherStatus? {
        var json = URL("http://home.kaczorzoo.net/api/weather/readings/recent").readText();

        var weatherStatus = Klaxon().parse<WeatherStatus>(json);

        return weatherStatus;
    }
}
