<script lang="ts" setup>
    import { ref } from 'vue';
    import { useCalendarStore } from '@/stores/calendarStore';
    import { setNextDayTimer } from '@/nextDayTimer';
    import NationalDayEntry from '@/models/calendar/national-day';

    const calendarStore = useCalendarStore();

    const nationalDays = ref([] as NationalDayEntry[]);
    const nationalDaysReady = ref(false);

    function loadNationalDays() {
        calendarStore.getNationalDays().then((data) => {
            nationalDays.value = data.sort((a, b) => a.name.localeCompare(b.name));

            nationalDaysReady.value = true;

            setNextDayTimer(loadNationalDays, 10000);
        });
    }

    loadNationalDays();
</script>

<template>
    <div
        class="national-days"
        v-if="nationalDaysReady">
        <ul>
            <li
                class="national-day"
                v-for="nationalDay in nationalDays">
                {{ nationalDay.name }}
            </li>
        </ul>
    </div>
</template>

<style>
    .national-days {
        background-color: #121212;
        border-radius: 10px;
        padding: 10px;
    }

    .national-day:not(:last-child) {
        padding-bottom: 2px;
    }
</style>
