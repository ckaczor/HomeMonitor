<script lang="ts" setup>
    import { ref } from 'vue';
    import { useCalendarStore } from '@/stores/calendarStore';
    import { startOfDay, isEqual } from 'date-fns';
    import NationalDayEntry from '@/models/calendar/national-day';

    const calendarStore = useCalendarStore();

    const nationalDays = ref([] as NationalDayEntry[]);
    const nationalDaysReady = ref(false);
    const loadedNationalDay = ref(null as Date | null);

    function loadNationalDays() {
        calendarStore.getNationalDays().then((data) => {
            nationalDays.value = data.sort((a, b) => a.name.localeCompare(b.name));

            loadedNationalDay.value = startOfDay(new Date());

            nationalDaysReady.value = true;
        });
    }

    loadNationalDays();

    setInterval(() => {
        if (loadedNationalDay.value && !isEqual(loadedNationalDay.value, startOfDay(new Date()))) {
            loadNationalDays();
        }
    }, 10 * 1000);
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
