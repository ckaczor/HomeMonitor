<script lang="ts" setup>
    import { capitalize, ref } from 'vue';
    import { useWeatherStore } from '@/stores/weatherStore';
    import { useLaundryStore } from '@/stores/laundryStore';
    import { usePowerStore } from '@/stores/powerStore';
    import { useHomeAssistantStore } from '@/stores/homeAssistantStore';
    import { useCalendarStore } from '@/stores/calendarStore';
    import { format, startOfDay, endOfDay } from 'date-fns';
    import CalendarDay from '@/models/calendar/calendar-day';

    const calendarDayCount = 7;

    const calendarReady = ref(false);

    const weatherStore = useWeatherStore();
    weatherStore.start();

    const laundryStore = useLaundryStore();
    laundryStore.start();

    const powerStore = usePowerStore();
    powerStore.start();

    const homeAssistantStore = useHomeAssistantStore();
    homeAssistantStore.start();

    const calendarStore = useCalendarStore();

    const currentTime = ref(new Date());
    const calendarDays = ref([] as CalendarDay[]);

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

    function loadCalendar() {
        const newCalendarDays = [] as CalendarDay[];

        calendarStore.getUpcoming(calendarDayCount).then((upcoming) => {
            const currentDay = startOfDay(currentTime.value);

            for (let i = 0; i < calendarDayCount; i++) {
                const day = new Date(currentDay);
                day.setDate(day.getDate() + i);

                const entries = upcoming.filter((entry) => {
                    const entryStart = startOfDay(entry.start);
                    const entryEnd = endOfDay(entry.end);

                    if (entry.isAllDay) {
                        return day > entryStart && day < entryEnd;
                    }

                    return day >= entryStart && day <= entryEnd;
                });

                newCalendarDays.push(new CalendarDay(day, entries));
            }

            calendarDays.value = newCalendarDays;

            calendarReady.value = true;
        });
    }

    loadCalendar();

    setInterval(() => currentTime.value = new Date(), 1000);
    setInterval(() => loadCalendar(), 60000);
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
                :class="laundryStore.current.washer.toString()">
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
                :class="laundryStore.current.dryer.toString()">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-tumble-dryer" />
                <div class="kiosk-device-text">
                    {{ laundryStore.current.dryer ? 'On' : 'Off' }}
                </div>
            </div>
            <div
                class="kiosk-garage-door text-center pt-4"
                v-if="homeAssistantStore?.garageState"
                v-on:click="homeAssistantStore.toggleGarage()">
                <v-icon
                    class="kiosk-device-icon"
                    :icon="homeAssistantStore.garageState === 'closed' ? 'mdi-garage' : 'mdi-garage-open'" />
                <div class="kiosk-device-text">
                    {{ capitalize(homeAssistantStore.garageState) }}
                </div>
            </div>
            <div
                class="kiosk-house-alarm text-center pt-4"
                v-if="homeAssistantStore?.houseAlarmState">
                <v-icon
                    class="kiosk-device-icon"
                    icon="mdi-shield-home" />
                <div class="kiosk-device-text">
                    {{ alarmState(homeAssistantStore.houseAlarmState) }}
                </div>
            </div>
        </div>
        <div class="kiosk-content">
            <div
                class="kiosk-calendar"
                v-if="calendarReady">
                <div class="kiosk-calendar-header">
                    {{ 'Next ' + calendarDayCount + ' Days' }}
                </div>
                <ul class="kiosk-calendar-day-list">
                    <li
                        class="kiosk-calendar-day-item"
                        v-for="calendarDay in calendarDays">
                        <div>
                            <span class="kiosk-calendar-day-item-number">
                                {{ format(calendarDay.date, 'dd') }}
                            </span>
                            <span class="kiosk-calendar-day-item-name">
                                {{ format(calendarDay.date, 'MMMM, EEEE') }}
                            </span>
                            <ul
                                class="kiosk-calendar-entry"
                                v-for="calendarEntry in calendarDay.entries">
                                <span>
                                    {{ calendarEntry.summary }}
                                </span>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
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

    .kiosk-content {
        height: 100%;
        max-height: 100vh;
        padding: 0;
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        grid-template-rows: repeat(2, 1fr);
        grid-auto-flow: row;
        grid-template-areas:
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
        font-size: 2.8rem;
        grid-area: kiosk-temperature;
    }

    .kiosk-humidity {
        font-size: 2.8rem;
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

    .kiosk-device-icon {
        font-size: 2.5rem;
    }

    .kiosk-device-text {
        font-size: 1.3rem;
    }

    .kiosk-calendar {
        grid-area: kiosk-calendar;
        background-color: #121212;
        margin: 10px;
        border-radius: 10px;
        display: flex;
        flex: 1;
        flex-direction: column;
    }

    .kiosk-calendar-header {
        font-size: 1.15em;
        padding-top: 10px;
        padding-bottom: 2px;
        text-align: center;
    }

    .kiosk-calendar-day-item-number {
        font-size: 1.25em;
        padding-right: 0.5em;
    }

    .kiosk-calendar-day-list {
        margin-left: 10px;
        overflow: auto;
        flex: 1;
    }

    .kiosk-calendar-day-item:not(:last-child) {
        padding-bottom: 2px;
    }

    .kiosk-calendar-day-item:first-of-type {
        color: #c75ec7;
    }

    .kiosk-calendar-day-item:not(:first-child) {
        padding-top: 4px;
    }

    .kiosk-calendar-entry {
        color: #ebebeb;
        padding-left: 2em;
        padding-bottom: 2px;
    }

    .true {
        color: #d09f27;
    }

    .false {
        color: #208b20;
    }
</style>
