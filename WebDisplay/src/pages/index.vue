<script lang="ts" setup>
    import CurrentWeather from '../components/CurrentWeather.vue';
    import CurrentPower from '../components/CurrentPower.vue';
</script>

<template>
    <v-container fluid class="container">
        <v-row dense align="start">
            <v-col cols="3">
                <CurrentWeather></CurrentWeather>
            </v-col>
            <v-col cols="3">
                <Almanac></Almanac>
            </v-col>
            <v-col cols="2">
                <CurrentPower></CurrentPower>
            </v-col>
            <v-col cols="2">
                <CurrentLaundryStatus></CurrentLaundryStatus>
            </v-col>
            <v-col cols="4">
                <WeatherSummary></WeatherSummary>
            </v-col>
            <v-col cols="2">
                <Indoor title="Upstairs" deviceName="main"></Indoor>
            </v-col>
            <v-col cols="2">
                <Indoor title="Downstairs" deviceName="basement"></Indoor>
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
