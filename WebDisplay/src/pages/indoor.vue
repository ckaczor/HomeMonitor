<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { createIndoorStore } from '@/stores/indoorStore';
    import { ConvertCToF } from '@/temperatureConverter';
    import { ConvertMillibarToInchesOfMercury } from '@/pressureConverter';
    import ValueChart from '../components/ValueChart.vue';
    import TimeRange from '@/components/TimeRange.vue';
    import TimeSpan from '@/models/time-span';

    const indoorStore = createIndoorStore('charts');

    const ready = ref(false);

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);

    const categories: number[] = [];

    const mainTemperatureSeries = { name: 'Upstairs', data: [] as number[] };
    const basementTemperatureSeries = { name: 'Downstairs', data: [] as number[] };

    const mainHumiditySeries = { name: 'Upstairs', data: [] as number[] };
    const basementHumiditySeries = { name: 'Downstairs', data: [] as number[] };

    const mainPressureSeries = { name: 'Upstairs', data: [] as number[] };
    const basementPressureSeries = { name: 'Downstairs', data: [] as number[] };

    const load = () => {
        ready.value = false;

        categories.length = 0;

        mainTemperatureSeries.data.length = 0;
        basementTemperatureSeries.data.length = 0;

        mainHumiditySeries.data.length = 0;
        basementHumiditySeries.data.length = 0;

        mainPressureSeries.data.length = 0;
        basementPressureSeries.data.length = 0;

        indoorStore.getReadingValueHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            groupedReadingsList.forEach((groupedReadings) => {
                if (groupedReadings.name === 'main') {
                    categories.push(new Date(groupedReadings.bucket).getTime());

                    mainTemperatureSeries.data.push(ConvertCToF(groupedReadings.averageTemperature));
                    mainHumiditySeries.data.push(groupedReadings.averageHumidity);
                    mainPressureSeries.data.push(ConvertMillibarToInchesOfMercury(groupedReadings.averagePressure));
                } else if (groupedReadings.name === 'basement') {
                    basementTemperatureSeries.data.push(ConvertCToF(groupedReadings.averageTemperature));
                    basementHumiditySeries.data.push(groupedReadings.averageHumidity);
                    basementPressureSeries.data.push(ConvertMillibarToInchesOfMercury(groupedReadings.averagePressure));
                }
            });

            ready.value = true;
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
                    sm="6"
                    cols="12">
                    <ValueChart
                        :ready="ready"
                        type="line"
                        title="Temperature"
                        unit="°F"
                        group="indoor"
                        :categories="categories"
                        :series="[mainTemperatureSeries, basementTemperatureSeries]"></ValueChart>
                </v-col>
                <v-col
                    sm="6"
                    cols="12">
                    <ValueChart
                        :ready="ready"
                        type="line"
                        title="Humidity"
                        unit="%"
                        group="indoor"
                        :categories="categories"
                        :series="[mainHumiditySeries, basementHumiditySeries]"></ValueChart>
                </v-col>
                <v-col
                    sm="6"
                    cols="12">
                    <ValueChart
                        :ready="ready"
                        type="line"
                        title="Pressure"
                        unit='"'
                        group="indoor"
                        :y-axis-decimal-points="1"
                        :categories="categories"
                        :series="[mainPressureSeries, basementPressureSeries]"></ValueChart>
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
