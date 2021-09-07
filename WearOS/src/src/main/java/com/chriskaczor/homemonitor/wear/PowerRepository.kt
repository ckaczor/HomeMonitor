package com.chriskaczor.homemonitor.wear

import com.beust.klaxon.Json
import com.beust.klaxon.Klaxon
import java.net.URL
import java.sql.Timestamp

data class PowerStatus(
    @Json(name = "generation")
    val generation: Int,

    @Json(name = "consumption")
    val consumption: Int,

    @Json(name = "timestamp")
    val timestamp: String
)

object PowerRepository {
    suspend fun getPowerStatus(): PowerStatus {
        val json = URL("http://home.kaczorzoo.net/api/power/status/recent").readText();

        val data = Klaxon().parse<PowerStatus>(json) ?: return powerStatus;

        powerStatus = powerStatus.copy(generation = data.generation, consumption = data.consumption, timestamp = data.timestamp);

        return powerStatus;
    }
}

var powerStatus =
    PowerStatus(generation = 0, consumption = 0, timestamp = Timestamp(System.currentTimeMillis()).toString())
