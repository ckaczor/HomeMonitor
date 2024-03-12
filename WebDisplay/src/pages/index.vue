<script lang="ts" setup></script>

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

    @media (min-width: 1024px) {
        .container {
            display: grid;
            grid-template-columns: repeat(5, max-content);
            grid-template-rows: repeat(3, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
                'current-weather current-weather almanac almanac current-power'
                'current-weather current-weather almanac almanac current-laundry-status'
                'upstairs downstairs . . .';
        }
    }

    @media (max-width: 1024px) {
        .container {
            display: grid;
            grid-template-columns: repeat(4, max-content);
            grid-template-rows: repeat(3, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
                'current-weather current-weather almanac almanac'
                'current-weather current-weather almanac almanac'
                'current-power current-laundry-status upstairs downstairs';
        }
    }

    @media (max-width: 768px) {
        .container {
            display: grid;
            grid-template-columns: repeat(1, 1fr);
            grid-template-rows: repeat(6, max-content);
            gap: 10px 0px;
            grid-template-areas:
                'current-weather'
                'almanac'
                'current-power'
                'current-laundry-status'
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

    .upstairs {
        grid-area: upstairs;
    }

    .downstairs {
        grid-area: downstairs;
    }
</style>
