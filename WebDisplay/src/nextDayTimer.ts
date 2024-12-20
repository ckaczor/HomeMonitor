import { addDays, startOfDay } from 'date-fns';

type CallbackFunction = () => void;

export function setNextDayTimer(callback: CallbackFunction, offset: number): void {
    const now = new Date();
    const startOfNextDay = startOfDay(addDays(now, 1));

    const millisecondsUntilNextDay = startOfNextDay.getTime() - now.getTime() + offset;

    setTimeout(callback, millisecondsUntilNextDay);
}
