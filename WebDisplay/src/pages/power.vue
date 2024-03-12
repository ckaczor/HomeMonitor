<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import { usePowerStore } from '@/stores/powerStore';
    import { useWeatherStore } from '@/stores/weatherStore';
    import ValueChart from '../components/ValueChart.vue';
    import TimeRange from '@/components/TimeRange.vue';
    import WeatherValueType from '@/models/weather/weather-value-type';
    import TimeSpan from '@/models/time-span';

    const powerStore = usePowerStore();
    const weatherStore = useWeatherStore();

    const powerReady = ref(false);
    const lightReady = ref(false);

    const powerCategories = ref<number[]>([]);
    const lightCategories = ref<number[]>([]);

    const generationSeries = ref({ name: 'Generation', data: [] as number[] });
    const consumptionSeries = ref({ name: 'Consumption', data: [] as number[] });

    const lightSeries = ref({ name: 'Average Light', data: [] as number[] });

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);

    const load = () => {
        powerReady.value = false;
        lightReady.value = false;

        powerStore.getReadingHistoryGrouped(start.value, end.value, 15).then((groupedReadingsList) => {
            powerCategories.value.length = 0;
            generationSeries.value.data.length = 0;
            consumptionSeries.value.data.length = 0;

            groupedReadingsList.forEach((groupedReadings) => {
                powerCategories.value.push(new Date(groupedReadings.bucket).getTime());

                generationSeries.value.data.push(groupedReadings.averageGeneration);
                consumptionSeries.value.data.push(groupedReadings.averageConsumption);
            });

            powerReady.value = true;
        });

        weatherStore.getReadingValueHistoryGrouped(WeatherValueType.Light, start.value, end.value, 15).then((groupedReadingsList) => {
            lightCategories.value.length = 0;
            lightSeries.value.data.length = 0;

            groupedReadingsList.forEach((groupedReadings) => {
                lightCategories.value.push(new Date(groupedReadings.bucket).getTime());

                lightSeries.value.data.push(groupedReadings.averageValue);
            });

            lightReady.value = true;
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
