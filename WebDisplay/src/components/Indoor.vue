<script setup lang="ts">
    import { createIndoorStore } from '@/stores/indoorStore';
    import { ConvertCToF } from '@/temperatureConverter';
    import { ConvertMillibarToInchesOfMercury } from '@/pressureConverter';

    const props = defineProps({
        title: { type: String, required: true },
        deviceName: { type: String, required: true }
    });

    const indoorStore = createIndoorStore(props.deviceName);
    indoorStore.start();

    const airQualityDescription = (airQualityIndex: number | undefined) => {
        if (!airQualityIndex) {
            return '';
        } else if (airQualityIndex >= 0 && airQualityIndex <= 50) {
            return 'Good';
        } else if (airQualityIndex >= 51 && airQualityIndex <= 100) {
            return 'Moderate';
        } else if (airQualityIndex >= 101 && airQualityIndex <= 150) {
            return 'Unhealthy for Sensitive Groups';
        } else if (airQualityIndex >= 151 && airQualityIndex <= 200) {
            return 'Unhealthy';
        } else if (airQualityIndex >= 201 && airQualityIndex <= 300) {
            return 'Very Unhealthy';
        } else if (airQualityIndex >= 301) {
            return 'Hazardous';
        }

        return '';
    };

    const airQualityClass = (airQualityIndex: number | undefined) => {
        if (!airQualityIndex) {
            return 'aqi-none';
        } else if (airQualityIndex >= 0 && airQualityIndex <= 50) {
            return 'aqi-green';
        } else if (airQualityIndex >= 51 && airQualityIndex <= 100) {
            return 'aqi-yellow';
        } else if (airQualityIndex >= 101 && airQualityIndex <= 150) {
            return 'aqi-orange';
        } else if (airQualityIndex >= 151 && airQualityIndex <= 200) {
            return 'aqi-red';
        } else if (airQualityIndex >= 201 && airQualityIndex <= 300) {
            return 'aqi-purple';
        } else if (airQualityIndex >= 301) {
            return 'aqi-maroon';
        }

        return '';
    };
</script>

<template>
    <DashboardItem :title="title">
        <div className="current">
            <div v-if="!indoorStore.current">Loading...</div>
            <table v-else>
                <tbody>
                    <tr>
                        <td className="header">Temperature</td>
                        <td>{{ ConvertCToF(indoorStore.current.temperature).toFixed(2) }}°F</td>
                    </tr>
                    <tr>
                        <td className="header">Humidity</td>
                        <td>{{ indoorStore.current.humidity.toFixed(2) }}%</td>
                    </tr>
                    <tr>
                        <td className="header">Pressure</td>
                        <td>{{ ConvertMillibarToInchesOfMercury(indoorStore.current.pressure).toFixed(2) }}"</td>
                    </tr>
                    <tr>
                        <td className="header">Air quality</td>
                        <td
                            :class="airQualityClass(indoorStore.current.airQualityIndex)"
                            :title="indoorStore.current.airQualityIndex.toString()">
                            {{ airQualityDescription(indoorStore.current.airQualityIndex) }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style scoped>
    .current {
        font-size: 14px;
        padding: 6px 12px;
    }

    .header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }

    .aqi-green {
        color: green;
    }

    .aqi-yellow {
        color: goldenrod;
    }

    .aqi-orange {
        color: orange;
    }

    .aqi-red {
        color: red;
    }

    .aqi-purple {
        color: purple;
    }

    .aqi-maroon {
        color: maroon;
    }
</style>
