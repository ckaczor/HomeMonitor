<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { createIndoorStore } from '@/stores/indoorStore';
    import { ConvertCToF } from '@/temperatureConverter';
    import { ConvertMillibarToInchesOfMercury } from '@/pressureConverter';
    import ValueChart from '@/components/ValueChart.vue';
    import TimeRange from '@/components/TimeRange.vue';
    import TimeSpan from '@/models/time-span';

    const indoorStore = createIndoorStore('charts');

    const ready = ref(false);

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);

    const mainTemperatureSeries = ref({ name: 'Upstairs', data: [] as number[][] });
    const basementTemperatureSeries = ref({ name: 'Downstairs', data: [] as number[][] });

    const mainHumiditySeries = ref({ name: 'Upstairs', data: [] as number[][] });
    const basementHumiditySeries = ref({ name: 'Downstairs', data: [] as number[][] });

    const mainPressureSeries = ref({ name: 'Upstairs', data: [] as number[][] });
    const basementPressureSeries = ref({ name: 'Downstairs', data: [] as number[][] });

    const load = () => {
        ready.value = false;

        indoorStore.getReadingValueHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            mainTemperatureSeries.value.data.length = 0;
            basementTemperatureSeries.value.data.length = 0;

            mainHumiditySeries.value.data.length = 0;
            basementHumiditySeries.value.data.length = 0;

            mainPressureSeries.value.data.length = 0;
            basementPressureSeries.value.data.length = 0;

            groupedReadingsList.forEach((groupedReadings) => {
                const date = new Date(groupedReadings.bucket).getTime();

                if (groupedReadings.name === 'main') {
                    mainTemperatureSeries.value.data.push([date, ConvertCToF(groupedReadings.averageTemperature)]);
                    mainHumiditySeries.value.data.push([date, groupedReadings.averageHumidity]);
                    mainPressureSeries.value.data.push([date, ConvertMillibarToInchesOfMercury(groupedReadings.averagePressure)]);
                } else if (groupedReadings.name === 'basement') {
                    basementTemperatureSeries.value.data.push([date, ConvertCToF(groupedReadings.averageTemperature)]);
                    basementHumiditySeries.value.data.push([date, groupedReadings.averageHumidity]);
                    basementPressureSeries.value.data.push([date, ConvertMillibarToInchesOfMercury(groupedReadings.averagePressure)]);
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
