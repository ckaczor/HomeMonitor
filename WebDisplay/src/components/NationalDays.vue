<script lang="ts" setup>
    import { ref } from 'vue';
    import { useCalendarStore } from '@/stores/calendarStore';
    import { setNextDayTimer } from '@/nextDayTimer';
    import NationalDayEntry from '@/models/calendar/national-day';

    const dialog = ref(false);
    const selectedNationalDay = ref({} as NationalDayEntry);

    const calendarStore = useCalendarStore();

    const nationalDays = ref([] as NationalDayEntry[]);
    const nationalDaysReady = ref(false);

    function loadNationalDays() {
        calendarStore.getNationalDays().then((data) => {
            nationalDays.value = data;

            nationalDaysReady.value = true;

            setNextDayTimer(loadNationalDays, 10000);
        });
    }

    function onNationalDayClick(nationalDay: NationalDayEntry) {
        selectedNationalDay.value = nationalDay;
        dialog.value = true;
    }

    loadNationalDays();
</script>

<template>
    <span>
        <div
            class="national-days"
            v-if="nationalDaysReady">
            <ul>
                <li
                    class="national-day"
                    v-for="nationalDay in nationalDays"
                    @click="onNationalDayClick(nationalDay)">
                    <span v-html="nationalDay.name"></span>
                    <v-icon
                        class="national-day-arrow"
                        icon="mdi-menu-right" />
                </li>
            </ul>
        </div>

        <v-dialog
            v-model="dialog"
            width="50%"
            theme="dark"
            opacity="0.85"
            scrim="black">
            <v-card>
                <template v-slot:title>
                    <span v-html="selectedNationalDay.name"></span>
                </template>
                <template v-slot:text>
                    <span v-html="selectedNationalDay.excerpt"></span>
                </template>
                <template v-slot:actions>
                    <v-btn
                        class="ms-auto"
                        text="Close"
                        @click="dialog = false"></v-btn>
                </template>
            </v-card>
        </v-dialog>
    </span>
</template>

<style>
    .national-days {
        background-color: #121212;
        border-radius: 10px;
        padding: 10px;
    }

    .national-day {
        list-style-type: none;
        line-height: 1.75rem;
    }

    .national-day-arrow {
        float: right;
    }
</style>
