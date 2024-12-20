<script lang="ts" setup>
    import { ref } from 'vue';
    import { useCalendarStore } from '@/stores/calendarStore';
    import { format, startOfDay, endOfDay } from 'date-fns';
    import { setNextDayTimer } from '@/nextDayTimer';
    import CalendarDay from '@/models/calendar/calendar-day';

    const props = defineProps(['days', 'refreshInterval']);

    const calendarStore = useCalendarStore();

    const calendarDays = ref([] as CalendarDay[]);
    const calendarReady = ref(false);

    function loadCalendar() {
        const newCalendarDays = [] as CalendarDay[];

        calendarStore.getUpcoming(props.days, true).then((upcoming) => {
            const currentDay = startOfDay(new Date());

            for (let i = 0; i < props.days; i++) {
                const day = new Date(currentDay);
                day.setDate(day.getDate() + i);

                const entries = upcoming
                    .filter((entry) => {
                        const entryStart = startOfDay(entry.start);
                        const entryEnd = endOfDay(entry.end);

                        if (entry.isAllDay) {
                            return day > entryStart && day < entryEnd;
                        }

                        return day >= entryStart && day <= entryEnd;
                    })
                    .sort((a, b) => {
                        if (a.isHoliday == b.isHoliday) {
                            return a.summary.localeCompare(b.summary);
                        }

                        return (b.isHoliday ? 1 : 0) - (a.isHoliday ? 1 : 0);
                    });

                newCalendarDays.push(new CalendarDay(day, entries));
            }

            calendarDays.value = newCalendarDays;

            calendarReady.value = true;

            setNextDayTimer(loadCalendar, 10000);
        });
    }

    loadCalendar();

    setInterval(loadCalendar, props.refreshInterval);
</script>

<template>
    <div
        class="calendar"
        v-if="calendarReady">
        <div class="calendar-header">
            {{ 'Next ' + days + ' Days' }}
        </div>
        <ul class="calendar-day-list">
            <li
                class="calendar-day-item"
                v-for="calendarDay in calendarDays">
                <div>
                    <div class="calendar-day-item-header">
                        <span class="calendar-day-item-number">
                            {{ format(calendarDay.date, 'dd') }}
                        </span>
                        <span class="calendar-day-item-name">
                            {{ format(calendarDay.date, 'EEEE') }}
                        </span>
                    </div>
                    <ul
                        class="calendar-entry"
                        v-for="calendarEntry in calendarDay.entries"
                        :class="{ 'calendar-holiday': calendarEntry.isHoliday }">
                        <span>
                            {{ calendarEntry.summary }}
                        </span>
                    </ul>
                </div>
            </li>
        </ul>
    </div>
</template>

<style>
    .calendar {
        background-color: #121212;
        border-radius: 10px;
        display: flex;
        flex: 1;
        flex-direction: column;
    }

    .calendar-header {
        font-size: 1.15em;
        padding-top: 10px;
        padding-bottom: 2px;
        text-align: center;
    }

    .calendar-day-item-header {
        display: flex;
        align-items: center;
    }

    .calendar-day-item-number {
        font-size: 1.25em;
        padding-right: 0.5em;
    }

    .calendar-day-item-name {
        font-size: 1em;
    }

    .calendar-day-list {
        margin-left: 10px;
        overflow: auto;
        flex: 1;
    }

    .calendar-day-item:not(:last-child) {
        padding-bottom: 2px;
    }

    .calendar-day-item:first-of-type {
        color: #c75ec7;
    }

    .calendar-day-item:not(:first-child) {
        padding-top: 4px;
    }

    .calendar-entry {
        color: #ebebeb;
        padding-left: 2em;
        padding-bottom: 2px;
    }

    .calendar-holiday {
        color: #5e83c7;
    }
</style>
