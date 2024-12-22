import { defineStore } from 'pinia';
import axios from 'axios';
import Environment from '@/environment';
import CalendarEntry from '@/models/calendar/calendar-entry';
import NationalDayEntry from '@/models/calendar/national-day';

export const useCalendarStore = defineStore('calendar', {
    state: () => {
        return {};
    },
    actions: {
        async getUpcoming(days: number, includeHolidays: boolean): Promise<CalendarEntry[]> {
            const response = await axios.get<CalendarEntry[]>(Environment.getUrlPrefix() + `:8081/api/calendar/calendar/upcoming?days=${days}&includeHolidays=${includeHolidays}`);

            return response.data;
        },
        async getNationalDays(): Promise<NationalDayEntry[]> {
            const response = await axios.get<NationalDayEntry[]>(Environment.getUrlPrefix() + `:8081/api/calendar/national-days/today?timezone=${Intl.DateTimeFormat().resolvedOptions().timeZone}`);

            return response.data;
        }
    }
});
