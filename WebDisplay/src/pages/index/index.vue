<script lang="ts" setup></script>

<template>
    <v-container
        fluid
        class="container">
        <v-card class="current-weather">
            <DashboardItem title="Weather">
                <CurrentWeather></CurrentWeather>
            </DashboardItem>
        </v-card>
        <v-card class="almanac">
            <DashboardItem title="Almanac">
                <Almanac></Almanac>
            </DashboardItem>
        </v-card>
        <v-card class="current-power">
            <DashboardItem title="Power">
                <CurrentPower></CurrentPower>
            </DashboardItem>
        </v-card>
        <v-card class="current-laundry-status">
            <DashboardItem title="Laundry">
                <CurrentLaundryStatus></CurrentLaundryStatus>
            </DashboardItem>
        </v-card>
        <v-card class="upstairs">
            <DashboardItem title="Upstairs">
                <Indoor deviceName="main"></Indoor>
            </DashboardItem>
        </v-card>
        <v-card class="downstairs">
            <DashboardItem title="Downstairs">
                <Indoor deviceName="basement"></Indoor>
            </DashboardItem>
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
            grid-template-columns: 360px 350px 210px 1fr;
            grid-template-rows: repeat(3, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
                'current-weather almanac current-laundry-status'
                'current-weather almanac current-power'
                'upstairs downstairs .';
        }
    }

    @media (max-width: 1024px) {
        .container {
            display: grid;
            grid-template-columns: repeat(4, 180px);
            grid-template-rows: repeat(2, max-content);
            gap: 15px 15px;
            grid-auto-flow: row;
            grid-template-areas:
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
