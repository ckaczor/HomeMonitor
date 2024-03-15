<script lang="ts" setup>
    import { ref, watch } from 'vue';
    import { createIndoorStore } from '@/stores/indoorStore';
    import ReadingsAggregate from '@/models/environment/readingsAggregate';
    import { ConvertPascalToInchesOfMercury } from '@/pressureConverter';
    import { ConvertCToF } from '@/temperatureConverter';

    const props = defineProps({
        name: { type: String, required: true },
        title: { type: String, required: true },
        start: { type: Date, required: true },
        end: { type: Date, required: true }
    });

    const readingAggregates = ref<ReadingsAggregate | undefined>();

    const indoorStore = createIndoorStore(props.name);

    const load = () => {
        indoorStore.getReadingsAggregate(props.name, props.start, props.end).then((newReadingAggregates) => {
            readingAggregates.value = newReadingAggregates;
        });
    };

    watch(
        () => [props.start, props.end],
        () => load
    );

    load();
</script>

<template>
    <DashboardItem :title="props.title">
        <div className="reading-summary">
            <div v-if="!readingAggregates">Loading...</div>
            <table v-else>
                <tbody>
                    <tr>
                        <th></th>
                        <th>Minimum</th>
                        <th>Average</th>
                        <th>Maximum</th>
                    </tr>
                    <tr>
                        <td class="reading-summary-header">Temperature</td>
                        <td>{{ ConvertCToF(readingAggregates!.minimumTemperature).toFixed(2) }}°F</td>
                        <td>{{ ConvertCToF(readingAggregates!.averageTemperature).toFixed(2) }}°F</td>
                        <td>{{ ConvertCToF(readingAggregates!.maximumTemperature).toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td class="reading-summary-header">Humidity</td>
                        <td>{{ readingAggregates!.minimumHumidity.toFixed(2) }}%</td>
                        <td>{{ readingAggregates!.averageHumidity.toFixed(2) }}%</td>
                        <td>{{ readingAggregates!.maximumHumidity.toFixed(2) }}%</td>
                    </tr>
                    <tr>
                        <td class="reading-summary-header">Pressure</td>
                        <td>{{ ConvertPascalToInchesOfMercury(readingAggregates!.minimumPressure).toFixed(2) }}"</td>
                        <td>{{ ConvertPascalToInchesOfMercury(readingAggregates!.averagePressure).toFixed(2) }}"</td>
                        <td>{{ ConvertPascalToInchesOfMercury(readingAggregates!.maximumPressure).toFixed(2) }}"</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style scoped>
    .reading-summary {
        font-size: 14px;
        padding: 6px 12px;
    }

    .reading-summary-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }

    th {
        padding-right: 30px;
    }
</style>
