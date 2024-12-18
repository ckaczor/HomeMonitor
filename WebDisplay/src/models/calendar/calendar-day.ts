import CalendarEntry from './calendar-entry';

export default class CalendarDay {
    date: Date;
    entries: CalendarEntry[];

    constructor(date: Date, entries: CalendarEntry[]) {
        this.date = date;
        this.entries = entries;
    }
}
