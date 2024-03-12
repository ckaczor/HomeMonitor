<script lang="ts" setup>
    import { ref, watch } from 'vue';
    import { useWeatherStore } from '@/stores/weatherStore';
    import WeatherAggregates from '@/models/weather/weather-aggregates';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import { ShortenWindDirection } from '@/windFormatter';

    const props = defineProps({
        start: { type: Date, required: true },
        end: { type: Date, required: true }
    });

    const weatherAggregates = ref<WeatherAggregates | undefined>();

    const weatherStore = useWeatherStore();

    weatherStore.getReadingAggregate(props.start, props.end).then((newWeatherAggregates) => {
        weatherAggregates.value = newWeatherAggregates;
    });

    watch(
        () => [props.start, props.end],
        () => {
            weatherStore.getReadingAggregate(props.start, props.end).then((newWeatherAggregates) => {
                weatherAggregates.value = newWeatherAggregates;
            });
        }
    );
</script>

<template>
    <DashboardItem title="Weather">
        <div className="weather-summary">
            <div v-if="!weatherAggregates">Loading...</div>
            <table v-else>
                <tbody>
                    <tr>
                        <th></th>
                        <th>Minimum</th>
                        <th>Average</th>
                        <th>Maximum</th>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Temperature</td>
                        <td>{{ weatherAggregates!.temperature.min.toFixed(2) }}°F</td>
                        <td>{{ weatherAggregates!.temperature.average.toFixed(2) }}°F</td>
                        <td>{{ weatherAggregates!.temperature.max.toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Humidity</td>
                        <td>{{ weatherAggregates!.humidity.min.toFixed(2) }}%</td>
                        <td>{{ weatherAggregates!.humidity.average.toFixed(2) }}%</td>
                        <td>{{ weatherAggregates!.humidity.max.toFixed(2) }}%</td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Pressure</td>
                        <td>{{ ConvertPascalToInchesOfMercury(weatherAggregates!.pressure.min).toFixed(2) }}"</td>
                        <td>{{ ConvertPascalToInchesOfMercury(weatherAggregates!.pressure.average).toFixed(2) }}"</td>
                        <td>{{ ConvertPascalToInchesOfMercury(weatherAggregates!.pressure.max).toFixed(2) }}"</td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Light</td>
                        <td>
                            {{ weatherAggregates!.light.min.toFixed(2) }}
                            lx
                        </td>
                        <td>
                            {{ weatherAggregates!.light.average.toFixed(2) }}
                            lx
                        </td>
                        <td>
                            {{ weatherAggregates!.light.max.toFixed(2) }}
                            lx
                        </td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Wind speed</td>
                        <td>
                            {{ weatherAggregates!.windSpeed.min.toFixed(2) }}
                            mph
                        </td>
                        <td>
                            {{ weatherAggregates!.windSpeed.average.toFixed(2) }}
                            mph
                        </td>
                        <td>
                            {{ weatherAggregates!.windSpeed.max.toFixed(2) }}
                            mph
                        </td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Wind direction</td>
                        <td></td>
                        <td>
                            {{ ShortenWindDirection(weatherAggregates!.windDirectionAverage) }}
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="weather-summary-header">Rain</td>
                        <td></td>
                        <td></td>
                        <td>{{ weatherAggregates!.rainTotal.toFixed(2) }}"</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style scoped>
    .weather-summary {
        font-size: 14px;
        padding: 6px 12px;
    }

    .weather-summary-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }

    th {
        padding-right: 30px;
    }
</style>
