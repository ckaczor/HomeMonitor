import express, { Request, Response } from 'express';
import { StatusCodes } from 'http-status-codes';
import * as DateHolidays from 'date-holidays';
import { Event } from './event';
import { DateTime } from 'luxon';

export const eventsRouter = express.Router();

function getHolidays(req: Request): DateHolidays.HolidaysTypes.Holiday[] {
    const country = req.query.country as string;
    const state = req.query.state as string;
    const year = parseInt(req.query.year as string, 10);
    const timezone = 'Etc/UTC';

    const dateHolidays = new DateHolidays.default();

    dateHolidays.init(country, state, {
        timezone: timezone
    });

    const holidays = dateHolidays.getHolidays(year);

    return holidays;
}

eventsRouter.get('/all', async (req: Request, res: Response) => {
    try {
        const timezone = req.query.timezone as string;
        
        const holidays = getHolidays(req);

        const events = holidays.map(holiday => new Event(holiday, timezone));

        return res.status(StatusCodes.OK).json(events);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});

eventsRouter.get('/next', async (req: Request, res: Response) => {
    try {
        const timezone = req.query.timezone as string;

        const holidays = getHolidays(req);

        const events = holidays.map(holiday => new Event(holiday, timezone));

        const now = DateTime.now();

        const nextEvent = events.find(event => event.date > now || event.isToday);

        if (!nextEvent) {
            return res.status(StatusCodes.OK).json(null);
        }

        return res.status(StatusCodes.OK).json(nextEvent);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});

eventsRouter.get('/future', async (req: Request, res: Response) => {
    try {
        const timezone = req.query.timezone as string;

        const holidays = getHolidays(req);

        const events = holidays.map(holiday => new Event(holiday, timezone));

        const now = DateTime.now();

        const futureEvents = events.filter(event => event.date > now || event.isToday);

        return res.status(StatusCodes.OK).json(futureEvents);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});