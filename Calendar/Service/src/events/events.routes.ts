import express, { Request, Response } from 'express';
import { StatusCodes } from 'http-status-codes';
import * as DateHolidays from 'date-holidays';
import { Event } from './event';

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

        const calendarEvents = holidays.map(holiday => new Event(holiday, timezone));

        return res.status(StatusCodes.OK).json(calendarEvents);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});

eventsRouter.get('/next', async (req: Request, res: Response) => {
    try {
        const timezone = req.query.timezone as string;
        const holidays = getHolidays(req);

        const nextHoliday = holidays.find(holiday => {
            const holidayDate = new Date(holiday.date);

            return holidayDate.getTime() > Date.now();
        });

        if (nextHoliday == null) {
            return res.status(StatusCodes.OK).json(null);
        }

        const calendarEvent = new Event(nextHoliday, timezone);

        return res.status(StatusCodes.OK).json(calendarEvent);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});

eventsRouter.get('/future', async (req: Request, res: Response) => {
    try {
        const timezone = req.query.timezone as string;
        const holidays = getHolidays(req);

        const futureHolidays = holidays.filter(holiday => {
            const holidayDate = new Date(holiday.date);

            return holidayDate.getTime() > Date.now();
        });

        const calendarEvents = futureHolidays.map(holiday => new Event(holiday, timezone));

        return res.status(StatusCodes.OK).json(calendarEvents);
    } catch (error) {
        return res.status(StatusCodes.INTERNAL_SERVER_ERROR).json({ error });
    }
});