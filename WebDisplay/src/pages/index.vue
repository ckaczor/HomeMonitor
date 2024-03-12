<script lang="ts" setup>
    import CurrentWeather from '../components/CurrentWeather.vue';
    import CurrentPower from '../components/CurrentPower.vue';
</script>

<template>
    <v-container
        fluid
        class="container">
        <v-card class="current-weather">
            <CurrentWeather></CurrentWeather>
        </v-card>
        <v-card class="almanac">
            <Almanac></Almanac>
        </v-card>
        <v-card class="current-power">
            <CurrentPower></CurrentPower>
        </v-card>
        <v-card class="current-laundry-status">
            <CurrentLaundryStatus></CurrentLaundryStatus>
        </v-card>
        <v-card class="weather-summary">
            <WeatherSummary></WeatherSummary>
        </v-card>
        <v-card class="upstairs">
            <Indoor
                title="Upstairs"
                deviceName="main"></Indoor>
        </v-card>
        <v-card class="downstairs">
            <Indoor
                title="Downstairs"
                deviceName="basement"></Indoor>
        </v-card>
    </v-container>
</template>

<style scoped>
    .container {
        height: 100%;
        background-color: #fafafa;
    }

    @media (min-width: 700px) {
        .container {
            display: grid;
            grid-template-columns: repeat(5, max-content);
            grid-template-rows: repeat(5, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
                'current-weather current-weather almanac almanac current-power'
                'current-weather current-weather almanac almanac current-laundry-status'
                'weather-summary weather-summary weather-summary upstairs downstairs'
                'weather-summary weather-summary weather-summary . .'
                '. . . . .';
        }
    }

    @media (max-width: 700px) {
        .container {
            padding: 7px;
            display: grid;
            grid-template-columns: repeat(1, max-content);
            grid-template-rows: repeat(7, max-content);
            gap: 10px 0px;
            grid-template-areas:
                'current-weather'
                'almanac'
                'current-power'
                'current-laundry-status'
                'weather-summary'
                'upstairs'
                'downstairs';
        }
    }

    .current-weather {
        grid-area: current-weather;
    }

    .almanac {
        grid-area: almanac;
    }

    .current-power {
        grid-area: current-power;
    }

    .current-laundry-status {
        grid-area: current-laundry-status;
    }

    .weather-summary {
        grid-area: weather-summary;
    }

    .upstairs {
        grid-area: upstairs;
    }

    .downstairs {
        grid-area: downstairs;
    }
</style>
