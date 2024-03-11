<script setup lang="ts">
    import { PropType } from 'vue';
    import { subHours, addDays, startOfDay, endOfDay, isSameDay } from 'date-fns';
    import TimeSpan from '@/models/time-span';

    const props = defineProps({
        timeSpan: { type: Number as PropType<TimeSpan>, required: true },
        start: { type: Date, required: true },
        end: { type: Date, required: true }
    });

    const emit = defineEmits<{
        change: [
            {
                timeSpan: TimeSpan;
                start: Date;
                end: Date;
            }
        ];

        refresh: [];
    }>();

    const timeSpanItems: { [value: number]: string } = {
        [TimeSpan.Last24Hours]: 'Last 24 hours',
        [TimeSpan.Day]: 'Day'
    };

    const addDay = (increment: number) => {
        const start = addDays(props.start, increment);
        const end = addDays(props.end, increment);

        emit('change', {
            timeSpan: props.timeSpan,
            start: start,
            end: end
        });
    };

    const handleTimeSpanChange = (timeSpan: TimeSpan) => {
        const date = new Date();

        if (timeSpan == TimeSpan.Last24Hours) {
            const end = date;
            const start = subHours(end, 24);

            emit('change', {
                timeSpan: timeSpan,
                start: start,
                end: end
            });
        } else {
            const end = endOfDay(date);
            const start = startOfDay(date);

            emit('change', {
                timeSpan: timeSpan,
                start: start,
                end: end
            });
        }
    };

    const showDayBack = () => {
        return props.timeSpan === TimeSpan.Day;
    };

    const showDayForward = () => {
        return props.timeSpan === TimeSpan.Day && !isSameDay(props.start, new Date());
    };

    const refresh = () => {
        const date = new Date();

        if (props.timeSpan == TimeSpan.Last24Hours) {
            const end = date;
            const start = subHours(end, 24);

            emit('change', {
                timeSpan: props.timeSpan,
                start: start,
                end: end
            });
        } else {
            emit('refresh');
        }
    };

    const setToday = () => {
        const date = new Date();

        const end = endOfDay(date);
        const start = startOfDay(date);

        emit('change', {
            timeSpan: props.timeSpan,
            start: start,
            end: end
        });
    };

    const setDate = (value: Date) => {
        const end = endOfDay(value);
        const start = startOfDay(value);

        emit('change', {
            timeSpan: props.timeSpan,
            start: start,
            end: end
        });
    };
</script>

<template>
    <v-app-bar
        class="bar"
        density="compact"
        fixed
        :elevation="0">
        <v-toolbar-items>
            <v-btn
                icon
                @click="refresh()">
                <v-icon>mdi-refresh</v-icon>
            </v-btn>
            <v-menu>
                <template v-slot:activator="{ props }">
                    <v-btn
                        class="ml-2"
                        color="primary"
                        v-bind="props">
                        {{ timeSpanItems[timeSpan] }}
                    </v-btn>
                </template>
                <v-list :value="props.timeSpan">
                    <v-list-item
                        v-for="(item, index) in timeSpanItems"
                        :key="index"
                        :value="index"
                        @click="handleTimeSpanChange(+index)">
                        <v-list-item-title>{{ item }}</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
            <v-btn
                class="ml-2"
                icon
                @click="addDay(-1)"
                v-show="showDayBack()">
                <v-icon>mdi-arrow-left</v-icon>
            </v-btn>
            <VueDatePicker
                class="date-picker"
                v-show="showDayBack()"
                auto-apply
                :model-value="props.start"
                @update:model-value="setDate"
                :clearable="false"
                :max-date="new Date()"
                :teleport="true"
                :enable-time-picker="false" />
            <v-btn
                icon
                @click="addDay(1)"
                v-show="showDayForward()">
                <v-icon>mdi-arrow-right</v-icon>
            </v-btn>
            <v-btn
                class="ml-2"
                @click="setToday()"
                v-show="showDayForward()">
                Today
            </v-btn>
        </v-toolbar-items>
    </v-app-bar>
</template>

<style scoped>
    .bar {
        background-color: #eeeeee;
        border-bottom: 1px solid lightgray;
    }

    .date-picker {
        display: flex;
        align-items: center;
    }
</style>
