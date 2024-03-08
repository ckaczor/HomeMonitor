<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { usePowerStore } from '@/stores/powerStore';
    import { useWeatherStore } from '@/stores/weatherStore';
    import ValueChart from '../components/ValueChart.vue';
    import WeatherValueType from '@/models/weather/weather-value-type';

    const powerStore = usePowerStore();
    const weatherStore = useWeatherStore();

    const powerReady = ref(false);
    const lightReady = ref(false);

    const powerCategories: number[] = [];
    const lightCategories: number[] = [];

    const generationSeries = { name: 'Generation', data: [] as number[] };
    const consumptionSeries = { name: 'Consumption', data: [] as number[] };

    const lightSeries = { name: 'Average Light', data: [] as number[] };

    const end = new Date();
    const start = subHours(end, 24);

    powerStore.getReadingHistoryGrouped(start, end, 15).then((groupedReadingsList) => {
        groupedReadingsList.forEach((groupedReadings) => {
            powerCategories.push(new Date(groupedReadings.bucket).getTime());

            generationSeries.data.push(groupedReadings.averageGeneration);
            consumptionSeries.data.push(groupedReadings.averageConsumption);
        });

        powerReady.value = true;
    });

    weatherStore.getReadingValueHistoryGrouped(WeatherValueType.Light, start, end, 15).then((groupedReadingsList) => {
        groupedReadingsList.forEach((groupedReadings) => {
            lightCategories.push(new Date(groupedReadings.bucket).getTime());

            lightSeries.data.push(groupedReadings.averageValue);
        });

        lightReady.value = true;
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
                sm="6"
                cols="12">
                <ValueChart
                    :ready="powerReady"
                    type="line"
                    title="Power"
                    unit=" W"
                    group="power"
                    :y-axis-decimal-points="0"
                    :value-decimal-points="0"
                    :categories="powerCategories"
                    :series="[generationSeries, consumptionSeries]"></ValueChart>
            </v-col>
            <v-col
                sm="6"
                cols="12">
                <ValueChart
                    :ready="lightReady"
                    type="line"
                    title="Light"
                    unit=" lx"
                    group="power"
                    :categories="lightCategories"
                    :series="[lightSeries]">
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
