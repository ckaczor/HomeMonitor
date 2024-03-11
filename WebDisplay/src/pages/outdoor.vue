<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import ValueChart from '../components/ValueChart.vue';
    import WindDirectionNumber from '@/models/weather/wind-direction-number';
    import TimeRange from '@/components/TimeRange.vue';
    import TimeSpan from '@/models/time-span';

    const weatherStore = useWeatherStore();

    const readingsReady = ref(false);
    const windReady = ref(false);

    const readingsCategories: number[] = [];
    const windCategories: number[] = [];

    const temperatureSeries = { name: 'Average Temperature', data: [] as number[] };
    const humiditySeries = { name: 'Average Humidity', data: [] as number[] };
    const pressureSeries = { name: 'Average Pressure', data: [] as number[] };
    const lightSeries = { name: 'Average Light', data: [] as number[] };
    const rainSeries = { name: 'Total Rain', data: [] as number[] };

    const windMinimumSeries = { name: 'Minimum', data: [] as number[] };
    const windAverageSeries = { name: 'Average', data: [] as number[] };
    const windMaximumSeries = { name: 'Maximum', data: [] as number[] };
    const windDirectionSeries = { name: 'Average Direction', data: [] as number[] };

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);

    const load = () => {
        readingsReady.value = false;
        readingsCategories.length = 0;
        temperatureSeries.data.length = 0;
        humiditySeries.data.length = 0;
        pressureSeries.data.length = 0;
        lightSeries.data.length = 0;
        rainSeries.data.length = 0;

        windReady.value = false;
        windCategories.length = 0;
        windMinimumSeries.data.length = 0;
        windAverageSeries.data.length = 0;
        windMaximumSeries.data.length = 0;
        windDirectionSeries.data.length = 0;

        weatherStore.getReadingHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            groupedReadingsList.forEach((groupedReadings) => {
                readingsCategories.push(new Date(groupedReadings.bucket).getTime());

                temperatureSeries.data.push(groupedReadings.averageTemperature);
                humiditySeries.data.push(groupedReadings.averageHumidity);
                pressureSeries.data.push(ConvertPascalToInchesOfMercury(groupedReadings.averagePressure));
                lightSeries.data.push(groupedReadings.averageLightLevel);
                rainSeries.data.push(groupedReadings.rainTotal);
            });

            readingsReady.value = true;
        });

        weatherStore.getWindHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            groupedReadingsList.forEach((groupedReadings) => {
                windCategories.push(new Date(groupedReadings.bucket).getTime());

                windMinimumSeries.data.push(groupedReadings.minimumSpeed);
                windAverageSeries.data.push(groupedReadings.averageSpeed);
                windMaximumSeries.data.push(groupedReadings.maximumSpeed);

                windDirectionSeries.data.push(groupedReadings.averageDirection);
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
                        :categories="readingsCategories"
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
                        :categories="readingsCategories"
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
                        :categories="readingsCategories"
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
                        :categories="readingsCategories"
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
                        :categories="readingsCategories"
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
                        :categories="windCategories"
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
                        :tick-amount="3"
                        :marker-size="3"
                        :line-size="0"
                        :y-axis-minimum="WindDirectionNumber.Min"
                        :y-axis-maximum="WindDirectionNumber.Max"
                        :categories="windCategories"
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
