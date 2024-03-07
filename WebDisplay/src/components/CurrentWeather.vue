<script setup lang="ts">
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';

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
            <table v-else>
                <tbody>
                    <tr>
                        <td className="weather-current-header">Temperature</td>
                        <td>{{ weatherStore.current?.Temperature?.toFixed(2) }}°F</td>
                    </tr>
                    <tr v-if="weatherStore.current?.HeatIndex">
                        <td className="weather-current-header">Heat index</td>
                        <td>{{ weatherStore.current?.HeatIndex?.toFixed(2) }}°F</td>
                    </tr>

                    <tr v-if="weatherStore.current?.WindChill">
                        <td className="weather-current-header">Wind chill</td>
                        <td>{{ weatherStore.current?.WindChill?.toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Humidity</td>
                        <td>{{ weatherStore.current?.Humidity?.toFixed(2) }}%</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Dew point</td>
                        <td>{{ weatherStore.current?.DewPoint?.toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Pressure</td>
                        <td>
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
                        <td>
                            {{ weatherStore.current?.WindSpeed?.toFixed(2) }}
                            mph {{ weatherStore.current?.WindDirection }}
                        </td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Rain</td>
                        <td>{{ weatherStore.current?.RainLastHour?.toFixed(2) }}" (last hour)</td>
                    </tr>
                    <tr>
                        <td className="weather-current-header">Light</td>
                        <td>
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
        padding: 6px 12px;
    }

    .weather-current-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
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
