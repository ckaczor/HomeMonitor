import { defineStore } from 'pinia';
import axios from 'axios';
import Environment from '@/environment';
import CalendarEntry from '@/models/calendar/calendar-entry';

export const useCalendarStore = defineStore('calendar', {
    state: () => {
        return {};
    },
    actions: {
        async getUpcoming(days: number): Promise<CalendarEntry[]> {
            const response = await axios.get<CalendarEntry[]>(Environment.getUrlPrefix() + `:8081/api/calendar/calendar/upcoming?days=${days}`);

            return response.data;
        }
    }
});
