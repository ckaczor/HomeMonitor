package com.chriskaczor.homemonitor.wear

import android.content.ComponentName
import android.os.Bundle
import android.widget.FrameLayout
import androidx.activity.ComponentActivity
import androidx.wear.tiles.manager.TileUiClient

class TilePreviewActivity : ComponentActivity() {
    lateinit var tileUiClient: TileUiClient

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        val rootLayout = findViewById<FrameLayout>(R.id.tile_container)

        tileUiClient = TileUiClient(
            context = this,
            component = ComponentName(this, PowerTileService::class.java),
            parentView = rootLayout
        )
        tileUiClient.connect()
    }

    override fun onDestroy() {
        super.onDestroy()
        tileUiClient.close()
    }
}
