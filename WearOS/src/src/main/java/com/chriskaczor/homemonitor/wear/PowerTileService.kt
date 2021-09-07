package com.chriskaczor.homemonitor.wear

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
private const val ID_IMAGE_GENERATION = "image_generation"
private const val ID_IMAGE_CONSUMPTION = "image_consumption"
private const val ID_CLICK_REFRESH = "click_refresh"

class PowerTileService : TileService() {
    private val serviceScope = CoroutineScope(Dispatchers.IO)

    override fun onTileRequest(requestParams: TileRequest) = serviceScope.future {
        val powerStatus = PowerRepository.getPowerStatus()

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
                ID_IMAGE_GENERATION,
                ImageResource.Builder()
                    .setAndroidResourceByResId(
                        AndroidImageResourceByResId.Builder()
                            .setResourceId(R.drawable.ic_sun)
                            .build()
                    ).build()
            )
            .addIdToImageMapping(
                ID_IMAGE_CONSUMPTION,
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

    private fun layout(goalProgress: PowerStatus, deviceParameters: DeviceParameters) =
        Box.Builder()
            .setWidth(expand())
            .setHeight(expand())
            .addContent(
                Column.Builder()
                    .addContent(
                        generationLayout(goalProgress.generation, deviceParameters)
                    )
                    .addContent(
                        consumptionLayout(goalProgress.consumption, deviceParameters)
                    )
                    .addContent(Spacer.Builder().setHeight(VERTICAL_SPACING_HEIGHT).build())
                    .addContent(refreshButton())
                    .build()
            ).build()

    private fun generationLayout(generation: Int, deviceParameters: DeviceParameters) =
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
                    .setResourceId(ID_IMAGE_GENERATION)
                    .build()
            )
            .addContent(
                Text.Builder()
                    .setText(generation.toString())
                    .setFontStyle(FontStyles.display3(deviceParameters).build())
                    .build()
            ).build()

    private fun consumptionLayout(consumption: Int, deviceParameters: DeviceParameters) =
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
                    .setResourceId(ID_IMAGE_CONSUMPTION)
                    .build()
            )
            .addContent(
                Text.Builder()
                    .setText(consumption.toString())
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
