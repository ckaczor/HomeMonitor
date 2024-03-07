<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import ValueChart from '../components/ValueChart.vue';

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

    const end = new Date();
    const start = subHours(end, 24);

    weatherStore.getReadingHistoryGrouped(start, end, 15).then((groupedReadingsList) => {
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

    weatherStore.getWindHistoryGrouped(start, end, 15).then((groupedReadingsList) => {
        groupedReadingsList.forEach((groupedReadings) => {
            windCategories.push(new Date(groupedReadings.bucket).getTime());

            windMinimumSeries.data.push(groupedReadings.minimumSpeed);
            windAverageSeries.data.push(groupedReadings.averageSpeed);
            windMaximumSeries.data.push(groupedReadings.maximumSpeed);
        });

        windReady.value = true;
    });
</script>

<template>
    <v-container
        fluid
        class="container">
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
                    :y-axis-decimal-points="2"
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
                    title="Wind"
                    unit=" MPH"
                    group="outdoor"
                    :y-axis-decimal-points="0"
                    :categories="windCategories"
                    :series="[windMinimumSeries, windAverageSeries, windMaximumSeries]">
                </ValueChart>
            </v-col>
        </v-row>
    </v-container>
</template>

<style scoped>
    .container {
        height: 100%;
        background-color: #fafafa;
    }
</style>
