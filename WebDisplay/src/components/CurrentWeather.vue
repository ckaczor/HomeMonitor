<script setup lang="ts">
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import { ShortenWindDirection } from '@/windFormatter';

    const weatherStore = useWeatherStore();
    weatherStore.start();

    const rotationClass = (pressureDifference: number | undefined) => {
        if (!pressureDifference) {
            return '';
        } else if (Math.abs(pressureDifference) <= 1.0) {
            return '';
        } else if (pressureDifference > 1.0 && pressureDifference <= 2.0) {
            return 'up-low';
        } else if (pressureDifference > 2.0) {
            return 'up-high';
        } else if (pressureDifference < -1.0 && pressureDifference >= -2.0) {
            return 'down-low';
        } else if (pressureDifference < -2.0) {
            return 'down-high';
        }

        return '';
    };
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
                                className="weather-current-header">
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
                            {{ weatherStore.current?.Pressure && ConvertPascalToInchesOfMercury(weatherStore.current?.Pressure)?.toFixed(2) }}"
                            <span
                                class="pressure-trend-arrow"
                                :class="rotationClass(weatherStore.current?.PressureDifferenceThreeHour)"
                                :title="'3 Hour Change: ' + weatherStore.current?.PressureDifferenceThreeHour?.toFixed(1)">
                                ➜
                            </span>
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
        display: inline-block;
        position: relative;
        left: 6px;
        transform: scale(1.25);
    }

    .down-high {
        transform: rotate(60deg) scale(1.25);
    }

    .down-low {
        transform: rotate(25deg) scale(1.25);
    }

    .up-high {
        transform: rotate(-60deg) scale(1.25);
    }

    .up-low {
        transform: rotate(-25deg) scale(1.25);
    }
</style>
