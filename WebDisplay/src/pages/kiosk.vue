<script lang="ts" setup>
    import { capitalize, ref } from 'vue';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { useLaundryStore } from '@/stores/laundryStore';
    import { usePowerStore } from '@/stores/powerStore';
    import { useHomeAssistantStore } from '@/stores/homeAssistantStore';

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
                v-if="weatherStore.current">
                {{ weatherStore.current?.Temperature?.toFixed(0) + '°F' }}
            </div>
            <div
                class="kiosk-humidity text-center pb-3"
                v-if="weatherStore.current">
                {{ weatherStore.current?.Humidity?.toFixed(0) + '%' }}
            </div>
            <div
                class="kiosk-generation text-center pt-4"
                v-if="powerStore.current">
                <v-icon
                    class="kiosk-device"
                    icon="mdi-solar-power-variant" />
                <div>
                    {{ (powerStore.current!.Generation < 0 ? 0 : powerStore.current!.Generation) + '&thinsp;W' }}
                </div>
            </div>
            <div
                class="kiosk-consumption text-center pt-4"
                v-if="powerStore.current">
                <v-icon
                    class="kiosk-device"
                    icon="mdi-home-lightning-bolt" />
                <div>
                    {{ powerStore.current.Consumption + '&thinsp;W' }}
                </div>
            </div>
            <div
                class="kiosk-washer text-center pt-4"
                v-if="laundryStore?.current?.washer !== undefined"
                :class="laundryStore.current.washer.toString()">
                <v-icon
                    class="kiosk-device"
                    icon="mdi-washing-machine" />
                <div>
                    {{ laundryStore.current.washer ? 'On' : 'Off' }}
                </div>
            </div>
            <div
                class="kiosk-dryer text-center pt-4"
                v-if="laundryStore?.current?.dryer !== undefined"
                :class="laundryStore.current.dryer.toString()">
                <v-icon
                    class="kiosk-device"
                    icon="mdi-tumble-dryer" />
                <div>
                    {{ laundryStore.current.dryer ? 'On' : 'Off' }}
                </div>
            </div>
            <div
                class="kiosk-garage-door text-center pt-4"
                v-if="homeAssistantStore?.garageState">
                <v-icon
                    class="kiosk-device"
                    :icon="homeAssistantStore.garageState === 'closed' ? 'mdi-garage' : 'mdi-garage-open'" />
                <div>
                    {{ capitalize(homeAssistantStore.garageState) }}
                </div>
            </div>
            <div
                class="kiosk-house-alarm text-center pt-4"
                v-if="homeAssistantStore?.houseAlarmState">
                <v-icon
                    class="kiosk-device"
                    icon="mdi-shield-home" />
                <div>
                    {{ capitalize(homeAssistantStore.houseAlarmState) }}
                </div>
            </div>
        </div>
    </v-container>
</template>

<style scoped>
    .kiosk-container {
        height: 100%;
        padding: 0;
        background-color: #020c25;
        color: #9acef1;
        display: grid;
        grid-template-columns: 250px 1fr;
        grid-template-rows: 1fr;
        gap: 15px 15px;
        grid-auto-flow: row;
        grid-template-areas:
            'kiosk-sidebar . '
            'kiosk-sidebar . '
            'kiosk-sidebar . ';
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
        background-color: #13213e;
        grid-area: kiosk-sidebar;

        display: grid;
        grid-template-columns: repeat(2, 50%);
        grid-template-rows: repeat(6, auto) 1fr;
        grid-column-gap: 0px;
        grid-row-gap: 0px;

        grid-template-areas:
            'kiosk-time kiosk-time'
            'kiosk-date kiosk-date'
            'kiosk-temperature kiosk-humidity'
            'kiosk-generation kiosk-consumption'
            'kiosk-washer kiosk-dryer'
            'kiosk-garage-door kiosk-house-alarm';
    }

    .kiosk-time {
        font-size: 2rem;
        grid-area: kiosk-time;
    }

    .kiosk-date {
        font-size: 1.1rem;
        grid-area: kiosk-date;
    }

    .kiosk-temperature {
        font-size: 2rem;
        grid-area: kiosk-temperature;
    }

    .kiosk-humidity {
        font-size: 2rem;
        grid-area: kiosk-humidity;
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

    .kiosk-device {
        font-size: 2.5rem;
    }

    .true {
        color: #d09f27;
    }

    .false {
        color: #208b20;
    }
</style>
