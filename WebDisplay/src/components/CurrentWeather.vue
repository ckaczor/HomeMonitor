<script setup lang="ts">
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ShortenWindDirection } from '@/windFormatter';
    import PressureTrendArrow from './PressureTrendArrow.vue';

    const weatherStore = useWeatherStore();
    weatherStore.start();
</script>

<template>
    <DashboardItem title="Weather">
        <div className="weather-current">
            <div v-if="!weatherStore.current">Loading...</div>
            <table
                v-else
                class="weather-table">
                <tbody>
                    <tr>
                        <td
                            className="weather-current-header"
                            width="1">
                            Temperature
                        </td>
                        <td :colspan="weatherStore.current?.HeatIndex || weatherStore.current?.WindChill ? 1 : 3">
                            {{ weatherStore.current?.Temperature?.toFixed(2) }}°F
                        </td>
                        <td>
                            <div
                                v-if="weatherStore.current?.HeatIndex || weatherStore.current?.WindChill"
                                className="weather-current-header ml-8">
                                Feels like
                            </div>
                        </td>
                        <td>
                            <div v-if="weatherStore.current?.HeatIndex">{{ weatherStore.current?.HeatIndex?.toFixed(2) }}°F</div>
                            <div v-if="weatherStore.current?.WindChill">{{ weatherStore.current?.WindChill?.toFixed(2) }}°F</div>
                        </td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Humidity</td>
                        <td colspan="3">{{ weatherStore.current?.Humidity?.toFixed(2) }}%</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Dew point</td>
                        <td colspan="3">{{ weatherStore.current?.DewPoint?.toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Pressure</td>
                        <td colspan="3">
                            {{ weatherStore.current?.Pressure && (weatherStore.current?.Pressure / 100).toFixed(2) }} mbar
                            <PressureTrendArrow
                                class="pressure-trend-arrow"
                                :pressureDifference="weatherStore.current?.PressureDifferenceThreeHour" />
                        </td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Wind</td>
                        <td colspan="3">
                            {{ weatherStore.current?.WindSpeed?.toFixed(2) }}
                            mph {{ ShortenWindDirection(weatherStore.current?.WindDirection) }}
                        </td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Rain</td>
                        <td colspan="3">{{ weatherStore.current?.RainLastHour?.toFixed(2) }}" (last hour)</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Light</td>
                        <td colspan="3">
                            {{ weatherStore.current?.LightLevel?.toFixed(2) }}
                            lx
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style>
    .weather-current {
        font-size: 14px;
        padding: 4px 12px;
    }

    .weather-current-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }

    .weather-table {
        width: 100%;
    }

    .pressure-trend-arrow {
        scale: 1.25;
    }
</style>
