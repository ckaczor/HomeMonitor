import { HolidaysTypes } from 'date-holidays';
import { DateTime, Interval } from 'luxon';

export class Event {
    name: string;
    date: DateTime;
    type: HolidaysTypes.HolidayType;
    durationUntil: {
        days: number;
        hours: number;
        minutes: number;
        seconds: number;
    } | null;

    constructor(holiday: HolidaysTypes.Holiday, timezone: string) {
        this.name = holiday.name;
        this.date = DateTime.fromFormat(holiday.date, 'yyyy-MM-dd HH:mm:ss', { zone: timezone });
        this.type = holiday.type;

        const duration = Interval.fromDateTimes(DateTime.now(), this.date).toDuration(['days', 'hours', 'minutes', 'seconds']);

        this.durationUntil = !duration.isValid ? null : {
            days: duration.days,
            hours: duration.hours,
            minutes: duration.minutes,
            seconds: Math.round(duration.seconds)
        };
    }
}