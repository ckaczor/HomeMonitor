<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import ValueChart from '@/components/ValueChart.vue';
    import TimeRange from '@/components/TimeRange.vue';
    import TimeSpan from '@/models/time-span';
    import { ConvertDegreesToShortLabel, ConvertWindDirectionToDegrees } from '@/windConverter';

    const weatherStore = useWeatherStore();

    const readingsReady = ref(false);
    const windReady = ref(false);

    const temperatureSeries = ref({ name: 'Average Temperature', data: [] as number[][] });
    const humiditySeries = ref({ name: 'Average Humidity', data: [] as number[][] });
    const pressureSeries = ref({ name: 'Average Pressure', data: [] as number[][] });
    const lightSeries = ref({ name: 'Average Light', data: [] as number[][] });
    const rainSeries = ref({ name: 'Total Rain', data: [] as number[][] });

    const windMinimumSeries = ref({ name: 'Minimum', data: [] as number[][] });
    const windAverageSeries = ref({ name: 'Average', data: [] as number[][] });
    const windMaximumSeries = ref({ name: 'Maximum', data: [] as number[][] });
    const windDirectionSeries = ref({ name: 'Average Direction', data: [] as number[][] });

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);

    const load = () => {
        readingsReady.value = false;
        windReady.value = false;

        weatherStore.getReadingHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            temperatureSeries.value.data.length = 0;
            humiditySeries.value.data.length = 0;
            pressureSeries.value.data.length = 0;
            lightSeries.value.data.length = 0;
            rainSeries.value.data.length = 0;

            groupedReadingsList.forEach((groupedReadings) => {
                const date = new Date(groupedReadings.bucket).getTime();

                temperatureSeries.value.data.push([date, groupedReadings.averageTemperature]);
                humiditySeries.value.data.push([date, groupedReadings.averageHumidity]);
                pressureSeries.value.data.push([date, ConvertPascalToInchesOfMercury(groupedReadings.averagePressure)]);
                lightSeries.value.data.push([date, groupedReadings.averageLightLevel]);
                rainSeries.value.data.push([date, groupedReadings.rainTotal]);
            });

            readingsReady.value = true;
        });

        weatherStore.getWindHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            windMinimumSeries.value.data.length = 0;
            windAverageSeries.value.data.length = 0;
            windMaximumSeries.value.data.length = 0;
            windDirectionSeries.value.data.length = 0;

            groupedReadingsList.forEach((groupedReadings) => {
                const date = new Date(groupedReadings.bucket).getTime();

                windMinimumSeries.value.data.push([date, groupedReadings.minimumSpeed]);
                windAverageSeries.value.data.push([date, groupedReadings.averageSpeed]);
                windMaximumSeries.value.data.push([date, groupedReadings.maximumSpeed]);

                windDirectionSeries.value.data.push([date, ConvertWindDirectionToDegrees(groupedReadings.averageDirection)]);
            });

            windReady.value = true;
        });
    };

    load();
</script>

<template>
    <v-container
        fluid
        class="container">
        <TimeRange
            :time-span="timeSpan"
            :start="start"
            :end="end"
            @change="
                (value) => {
                    timeSpan = value.timeSpan;
                    start = value.start;
                    end = value.end;

                    load();
                }
            "
            @refresh="load"></TimeRange>
        <div class="content">
            <v-row
                dense
                align="start">
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="readingsReady"
                        type="line"
                        title="Temperature"
                        unit="°F"
                        group="outdoor"
                        :series="[temperatureSeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="readingsReady"
                        type="line"
                        title="Humidity"
                        unit="%"
                        group="outdoor"
                        :series="[humiditySeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="readingsReady"
                        type="line"
                        title="Pressure"
                        unit='"'
                        group="outdoor"
                        :y-axis-decimal-points="1"
                        :series="[pressureSeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="readingsReady"
                        type="line"
                        title="Light"
                        unit=" lx"
                        group="outdoor"
                        :series="[lightSeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="readingsReady"
                        type="line"
                        title="Rain"
                        unit='"'
                        group="outdoor"
                        :stepline="true"
                        :y-axis-decimal-points="3"
                        :value-decimal-points="2"
                        :series="[rainSeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="windReady"
                        type="line"
                        title="Wind Speed"
                        unit=" MPH"
                        group="outdoor"
                        :y-axis-decimal-points="0"
                        :series="[windMaximumSeries, windAverageSeries, windMinimumSeries]">
                    </ValueChart>
                </v-col>
                <v-col
                    sm="4"
                    xl="6"
                    cols="12">
                    <ValueChart
                        :ready="windReady"
                        type="line"
                        title="Wind Direction"
                        unit=""
                        group="outdoor"
                        :tick-amount="4"
                        :y-axis-minimum="0"
                        :y-axis-maximum="360"
                        :y-axis-label-formatter="ConvertDegreesToShortLabel"
                        :y-axis-value-formatter="ConvertDegreesToShortLabel"
                        :series="[windDirectionSeries]">
                    </ValueChart>
                </v-col>
            </v-row>
        </div>
    </v-container>
</template>

<style scoped>
    .container {
        height: 100%;
        background-color: #fafafa;
        padding: 0;
    }

    .content {
        padding: 10px;
    }
</style>
