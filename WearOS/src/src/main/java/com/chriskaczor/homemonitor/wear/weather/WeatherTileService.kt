package com.chriskaczor.homemonitor.wear.weather

import androidx.core.content.ContextCompat
import androidx.wear.tiles.ActionBuilders
import androidx.wear.tiles.ColorBuilders.argb
import androidx.wear.tiles.DeviceParametersBuilders.DeviceParameters
import androidx.wear.tiles.DimensionBuilders.dp
import androidx.wear.tiles.DimensionBuilders.expand
import androidx.wear.tiles.LayoutElementBuilders.*
import androidx.wear.tiles.ModifiersBuilders.*
import androidx.wear.tiles.RequestBuilders.ResourcesRequest
import androidx.wear.tiles.RequestBuilders.TileRequest
import androidx.wear.tiles.ResourceBuilders.*
import androidx.wear.tiles.TileBuilders.Tile
import androidx.wear.tiles.TileService
import androidx.wear.tiles.TimelineBuilders.Timeline
import androidx.wear.tiles.TimelineBuilders.TimelineEntry
import com.chriskaczor.homemonitor.wear.R
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.cancel
import kotlinx.coroutines.guava.future

private const val RESOURCES_VERSION = "1"

// dimensions
private val BUTTON_SIZE = dp(48f)
private val BUTTON_RADIUS = dp(24f)
private val BUTTON_PADDING = dp(12f)
private val VERTICAL_SPACING_HEIGHT = dp(8f)

// identifiers
private const val ID_IMAGE_REFRESH = "image_refresh"
private const val ID_IMAGE_TEMPERATURE = "image_temperature"
private const val ID_IMAGE_HUMIDITY = "image_humidity"
private const val ID_IMAGE_PRESSURE = "image_pressure"
private const val ID_CLICK_REFRESH = "click_refresh"

class WeatherTileService : TileService() {
    private val serviceScope = CoroutineScope(Dispatchers.IO)

    override fun onTileRequest(requestParams: TileRequest) = serviceScope.future {
        val powerStatus = WeatherRepository.getWeatherStatus()

        val deviceParams = requestParams.deviceParameters!!

        Tile.Builder()
            .setResourcesVersion(RESOURCES_VERSION)
            .setTimeline(
                Timeline.Builder()
                    .addTimelineEntry(
                        TimelineEntry.Builder()
                            .setLayout(
                                Layout.Builder()
                                    .setRoot(
                                        layout(powerStatus, deviceParams)
                                    ).build()
                            ).build()
                    ).build()
            ).build()
    }

    override fun onResourcesRequest(requestParams: ResourcesRequest) = serviceScope.future {
        Resources.Builder()
            .setVersion(RESOURCES_VERSION)
            .addIdToImageMapping(
                ID_IMAGE_REFRESH,
                ImageResource.Builder()
                    .setAndroidResourceByResId(
                        AndroidImageResourceByResId.Builder()
                            .setResourceId(R.drawable.ic_refresh)
                            .build()
                    ).build()
            )
            .addIdToImageMapping(
                ID_IMAGE_HUMIDITY,
                ImageResource.Builder()
                    .setAndroidResourceByResId(
                        AndroidImageResourceByResId.Builder()
                            .setResourceId(R.drawable.ic_humidity)
                            .build()
                    ).build()
            )
            .addIdToImageMapping(
                ID_IMAGE_TEMPERATURE,
                ImageResource.Builder()
                    .setAndroidResourceByResId(
                        AndroidImageResourceByResId.Builder()
                            .setResourceId(R.drawable.ic_temperature)
                            .build()
                    ).build()
            )
            .addIdToImageMapping(
                ID_IMAGE_PRESSURE,
                ImageResource.Builder()
                    .setAndroidResourceByResId(
                        AndroidImageResourceByResId.Builder()
                            .setResourceId(R.drawable.ic_plug)
                            .build()
                    ).build()
            ).build()
    }

    override fun onDestroy() {
        super.onDestroy()
        serviceScope.cancel()
    }

    private fun layout(goalProgress: WeatherStatus?, deviceParameters: DeviceParameters) =
        Box.Builder()
            .setWidth(expand())
            .setHeight(expand())
            .addContent(
                Column.Builder()
                    .addContent(
                        temperatureLayout(goalProgress?.temperature ?: -1.0, deviceParameters)
                    )
                    .addContent(
                        humidityLayout(goalProgress?.humidity ?: -1.0, deviceParameters)
                    )
                    .addContent(
                        pressureLayout((goalProgress?.pressure ?: -100.0) / 100, deviceParameters)
                    )
                    .addContent(Spacer.Builder().setHeight(VERTICAL_SPACING_HEIGHT).build())
                    .addContent(refreshButton())
                    .build()
            ).build()

    private fun temperatureLayout(temperature: Double, deviceParameters: DeviceParameters) =
        Row.Builder()
            .addContent(
                Image.Builder()
                    .setHeight(dp(36f))
                    .setWidth(dp(36f))
                    .setModifiers(
                        Modifiers.Builder()
                            .setPadding(
                                Padding.Builder()
                                    .setStart(dp(0f))
                                    .setEnd(dp(10f))
                                    .setTop(dp(1f))
                                    .setBottom(dp(0f))
                                    .build()
                            )
                            .build()
                    )
                    .setResourceId(ID_IMAGE_TEMPERATURE)
                    .build()
            )
            .addContent(
                Text.Builder()
                    .setText(String.format("%.01f", temperature))
                    .setFontStyle(FontStyles.display3(deviceParameters).build())
                    .build()
            ).build()

    private fun humidityLayout(humidity: Double, deviceParameters: DeviceParameters) =
        Row.Builder()
            .addContent(
                Image.Builder()
                    .setHeight(dp(36f))
                    .setWidth(dp(36f))
                    .setModifiers(
                        Modifiers.Builder()
                            .setPadding(
                                Padding.Builder()
                                    .setStart(dp(0f))
                                    .setEnd(dp(10f))
                                    .setTop(dp(1f))
                                    .setBottom(dp(0f))
                                    .build()
                            )
                            .build()
                    )
                    .setResourceId(ID_IMAGE_HUMIDITY)
                    .build()
            )
            .addContent(
                Text.Builder()
                    .setText(if (humidity <= 0) "0" else String.format("%.01f", humidity))
                    .setFontStyle(FontStyles.display3(deviceParameters).build())
                    .build()
            ).build()

    private fun pressureLayout(pressure: Double, deviceParameters: DeviceParameters) =
        Row.Builder()
            .addContent(
                Image.Builder()
                    .setHeight(dp(36f))
                    .setWidth(dp(36f))
                    .setModifiers(
                        Modifiers.Builder()
                            .setPadding(
                                Padding.Builder()
                                    .setStart(dp(0f))
                                    .setEnd(dp(10f))
                                    .setTop(dp(1f))
                                    .setBottom(dp(0f))
                                    .build()
                            )
                            .build()
                    )
                    .setResourceId(ID_IMAGE_PRESSURE)
                    .build()
            )
            .addContent(
                Text.Builder()
                    .setText(if (pressure <= 0) "0" else String.format("%.01f", pressure))
                    .setFontStyle(FontStyles.display3(deviceParameters).build())
                    .build()
            ).build()

    private fun refreshButton() =
        Image.Builder()
            .setWidth(BUTTON_SIZE)
            .setHeight(BUTTON_SIZE)
            .setResourceId(ID_IMAGE_REFRESH)
            .setModifiers(
                Modifiers.Builder()
                    .setPadding(
                        Padding.Builder()
                            .setStart(BUTTON_PADDING)
                            .setEnd(BUTTON_PADDING)
                            .setTop(BUTTON_PADDING)
                            .setBottom(BUTTON_PADDING)
                            .build()
                    )
                    .setBackground(
                        Background.Builder()
                            .setCorner(Corner.Builder().setRadius(BUTTON_RADIUS).build())
                            .setColor(argb(ContextCompat.getColor(this, R.color.primaryDark)))
                            .build()
                    )
                    .setClickable(
                        Clickable.Builder()
                            .setId(ID_CLICK_REFRESH)
                            .setOnClick(ActionBuilders.LoadAction.Builder().build())
                            .build()
                    ).build()
            ).build()
}
