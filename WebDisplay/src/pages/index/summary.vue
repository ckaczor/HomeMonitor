<script lang="ts" setup>
    import { ref } from 'vue';
    import { subHours } from 'date-fns';
    import TimeSpan from '@/models/time-span';
    import TimeRange from '@/components/TimeRange.vue';
    import WeatherSummary from '@/components/WeatherSummary.vue';

    const end = ref(new Date());
    const start = ref(subHours(end.value, 24));
    const timeSpan = ref(TimeSpan.Last24Hours);
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
                }
            "></TimeRange>
        <v-card class="weather-summary">
            <WeatherSummary
                :start="start"
                :end="end"></WeatherSummary>
        </v-card>
        <v-card class="main-summary">
            <IndoorSummary
                name="main"
                title="Upstairs"
                :start="start"
                :end="end"></IndoorSummary>
        </v-card>
        <v-card class="basement-summary">
            <IndoorSummary
                name="basement"
                title="Downstairs"
                :start="start"
                :end="end"></IndoorSummary>
        </v-card>
    </v-container>
</template>

<style scoped>
    .container {
        height: 100%;
        background-color: #fafafa;
    }

    @media (min-width: 1024px) {
        .container {
            display: grid;
            grid-template-columns: 425px max-content;
            grid-template-rows: repeat(2, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
                'weather-summary main-summary'
                'weather-summary basement-summary';
        }
    }

    @media (max-width: 1024px) {
        .container {
            display: grid;
            grid-template-columns: 425px;
            grid-template-rows: repeat(3, max-content);
            gap: 15px 0px;
            grid-auto-flow: row;
            grid-template-areas:
                'weather-summary'
                'main-summary'
                'basement-summary';
        }
    }

    @media (max-width: 768px) {
        .container {
            display: grid;
            grid-template-columns: 1fr;
            grid-template-rows: repeat(3, max-content);
            gap: 15px 0px;
            grid-auto-flow: row;
            grid-template-areas:
                'weather-summary'
                'main-summary'
                'basement-summary';
        }
    }

    .weather-summary {
        grid-area: weather-summary;
    }

    .main-summary {
        grid-area: main-summary;
    }

    .basement-summary {
        grid-area: basement-summary;
    }
</style>
