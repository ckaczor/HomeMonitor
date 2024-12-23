<script lang="ts" setup>
    import { capitalize, ref } from 'vue';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { useLaundryStore } from '@/stores/laundryStore';
    import { usePowerStore } from '@/stores/powerStore';
    import { useHomeAssistantStore } from '@/stores/homeAssistantStore';
    import { ShortenWindDirection } from '@/windFormatter';
    import CalendarAgenda from '@/components/CalendarAgenda.vue';
    import LongPressButton from '@/components/LongPressButton.vue';
    import PressureTrendArrow from '@/components/PressureTrendArrow.vue';

    const showFeelsLike = ref(false);

    const weatherStore = useWeatherStore();
    weatherStore.start();

    const laundryStore = useLaundryStore();
    laundryStore.start();

    const powerStore = usePowerStore();
    powerStore.start();

    const homeAssistantStore = useHomeAssistantStore();
    homeAssistantStore.start();

    const currentTime = ref(new Date());

    const timeFormatter = new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: '2-digit' });
    const dateFormatter = new Intl.DateTimeFormat('en-US', { weekday: 'long', month: 'long', day: 'numeric' });

    function alarmState(state: string): string {
        switch (state) {
            case 'armed_home':
                return 'Armed';
            case 'armed_away':
                return 'Armed';
            case 'disarmed':
                return 'Disarmed';
            default:
                return 'Unknown';
        }
    }

    function getTemperature(): string {
        if (showFeelsLike.value && weatherStore.current?.WindChill) {
            return weatherStore.current?.WindChill?.toFixed(0) + '°';
        } else if (showFeelsLike.value && weatherStore.current?.HeatIndex) {
            return weatherStore.current?.HeatIndex?.toFixed(0) + '°';
        } else {
            return weatherStore.current?.Temperature?.toFixed(0) + '°';
        }
    }

    function getTemperatureClass(): string {
        if (showFeelsLike.value && weatherStore.current?.WindChill) {
            return 'temperature-wind-chill';
        } else if (showFeelsLike.value && weatherStore.current?.HeatIndex) {
            return 'temperature-heat-index';
        } else {
            return '';
        }
    }

    setInterval(() => (currentTime.value = new Date()), 1000);
</script>

<template>
    <v-container
        fluid
        class="kiosk-container">
        <div class="kiosk-sidebar">
            <div
                name="kiosk-time"
                class="kiosk-time text-center">
                {{ timeFormatter.format(currentTime) }}
            </div>
            <div class="kiosk-date text-center pb-4">
                {{ dateFormatter.format(currentTime) }}
            </div>
            <div
                class="kiosk-temperature text-center pb-3"
                :class="getTemperatureClass()"
                @click="showFeelsLike = !showFeelsLike"
                v-if="weatherStore.current">
                <div class="temperature">
                    <span class="temperature-value">{{ getTemperature() }}</span>
                    <span class="temperature-label">{{ showFeelsLike ? 'Feels Like' : 'Actual' }}</span>
                </div>
            </div>
            <div
                class="kiosk-humidity text-center pb-3"
                v-if="weatherStore.current">
                {{ weatherStore.current?.Humidity?.toFixed(0) + '%' }}
            </div>
            <div
                class="kiosk-wind text-center pb-3"
                v-if="weatherStore.current">
                {{ weatherStore.current?.WindSpeed?.toFixed(0) + '&hairsp;' + ShortenWindDirection(weatherStore.current?.WindDirection) }}
            </div>
            <div
                class="kiosk-pressure text-center pb-3"
                v-if="weatherStore.current">
                <span>
                    {{ (weatherStore.current?.Pressure! / 100).toFixed(0) }}
                </span>
                <PressureTrendArrow :pressureDifference="weatherStore.current?.PressureDifferenceThreeHour" />
            </div>
            <div
                class="kiosk-generation text-center pt-4"
                v-if="powerStore.current">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-solar-power-variant" />
                <div class="kiosk-device-text">
                    {{ (powerStore.current!.Generation < 0 ? 0 : powerStore.current!.Generation) + '&thinsp;W' }}
                </div>
            </div>
            <div
                class="kiosk-consumption text-center pt-4"
                v-if="powerStore.current">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-home-lightning-bolt" />
                <div class="kiosk-device-text">
                    {{ powerStore.current.Consumption + '&thinsp;W' }}
                </div>
            </div>
            <div
                class="kiosk-washer text-center pt-4"
                v-if="laundryStore?.current?.washer !== undefined"
                :class="laundryStore.current.washer ? 'warning' : 'normal'">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-washing-machine" />
                <div class="kiosk-device-text">
                    {{ laundryStore.current.washer ? 'On' : 'Off' }}
                </div>
            </div>
            <div
                class="kiosk-dryer text-center pt-4"
                v-if="laundryStore?.current?.dryer !== undefined"
                :class="laundryStore.current.dryer ? 'warning' : 'normal'">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-tumble-dryer" />
                <div class="kiosk-device-text">
                    {{ laundryStore.current.dryer ? 'On' : 'Off' }}
                </div>
            </div>
            <LongPressButton
                class="kiosk-garage-door text-center pt-4"
                v-if="homeAssistantStore?.garageState"
                :duration="2000"
                :increment="100"
                :progress-size="38"
                v-on:longPress="homeAssistantStore.toggleGarage()"
                :class="homeAssistantStore.garageState === 'closed' ? 'normal' : 'warning'">
                <v-icon
                    class="kiosk-device-icon"
                    :icon="homeAssistantStore.garageState === 'closed' ? 'mdi-garage' : 'mdi-garage-open'" />
                <div class="kiosk-device-text">
                    {{ capitalize(homeAssistantStore.garageState) }}
                </div>
            </LongPressButton>
            <div
                class="kiosk-house-alarm text-center pt-4"
                v-if="homeAssistantStore?.houseAlarmState"
                :class="homeAssistantStore.houseAlarmState === 'disarmed' ? 'normal' : 'warning'">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-shield-home" />
                <div class="kiosk-device-text">
                    {{ alarmState(homeAssistantStore.houseAlarmState) }}
                </div>
            </div>
        </div>
        <div class="kiosk-content">
            <CalendarAgenda
                class="kiosk-calendar"
                days="7"
                :refresh-interval="5 * 60 * 1000" />
            <NationalDays class="kiosk-national-days" />
        </div>
    </v-container>
</template>

<style scoped>
    .kiosk-container {
        height: 100%;
        padding: 0;
        background-color: #212428;
        color: #ebebeb;
        display: grid;
        grid-template-columns: 250px 1fr;
        grid-template-rows: 1fr;
        grid-auto-flow: row;
        grid-template-areas: 'kiosk-sidebar kiosk-content';
    }

    .kiosk-row {
        padding-top: 5px;
        display: flex;
        flex-direction: row;
        justify-content: space-around;
        align-items: center;
    }

    .kiosk-sidebar {
        padding: 5px;
        background-color: #121212;
        grid-area: kiosk-sidebar;

        display: grid;
        grid-template-columns: repeat(2, 50%);
        grid-template-rows: repeat(7, auto) 1fr;
        grid-column-gap: 0px;
        grid-row-gap: 0px;

        grid-template-areas:
            'kiosk-time kiosk-time'
            'kiosk-date kiosk-date'
            'kiosk-temperature kiosk-humidity'
            'kiosk-wind kiosk-pressure'
            'kiosk-generation kiosk-consumption'
            'kiosk-washer kiosk-dryer'
            'kiosk-garage-door kiosk-house-alarm';
    }

    .kiosk-content {
        height: 100%;
        max-height: calc(100vh - 30px);
        padding: 10px;
        gap: 10px;
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        grid-template-rows: repeat(4, 25%);
        grid-auto-flow: row;
        grid-template-areas:
            'kiosk-calendar kiosk-national-days kiosk-national-days'
            'kiosk-calendar . .'
            'kiosk-calendar . .'
            'kiosk-calendar . .';
    }

    .kiosk-time {
        font-size: 2.8rem;
        grid-area: kiosk-time;
    }

    .kiosk-date {
        font-size: 1.25rem;
        grid-area: kiosk-date;
    }

    .kiosk-temperature {
        font-size: 2.9rem;
        grid-area: kiosk-temperature;
    }

    .kiosk-humidity {
        font-size: 2.9rem;
        grid-area: kiosk-humidity;
    }

    .kiosk-wind {
        font-size: 2rem;
        grid-area: kiosk-wind;
    }

    .kiosk-pressure {
        font-size: 2rem;
        grid-area: kiosk-pressure;
    }

    .kiosk-generation {
        grid-area: kiosk-generation;
    }

    .kiosk-consumption {
        grid-area: kiosk-consumption;
    }

    .kiosk-washer {
        grid-area: kiosk-washer;
    }

    .kiosk-dryer {
        grid-area: kiosk-dryer;
    }

    .kiosk-garage-door {
        grid-area: kiosk-garage-door;
    }

    .kiosk-house-alarm {
        grid-area: kiosk-house-alarm;
    }

    .kiosk-device-icon {
        font-size: 2.5rem;
    }

    .kiosk-device-text {
        font-size: 1.3rem;
    }

    .kiosk-calendar {
        grid-area: kiosk-calendar;
    }

    .kiosk-national-days {
        grid-area: kiosk-national-days;
        scrollbar-width: thin;
    }

    .warning {
        color: #d09f27;
    }

    .normal {
        color: #208b20;
    }

    .temperature {
        display: flex;
        flex-direction: column;
    }

    .temperature-value {
        height: calc(1em + 10px);
    }

    .temperature-label {
        font-size: 10pt;
    }

    .temperature-wind-chill {
        color: #4d4dff;
    }

    .temperature-heat-index {
        color: #ff4d4d;
    }
</style>
