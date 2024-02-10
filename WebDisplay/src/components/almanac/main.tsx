import './main.scss';

import { useEffect, useState } from 'react';
import * as SunCalc from 'suncalc';
import DashboardItem from '../dashboard-item/main';
import WeatherService from '../../services/weather/main';
import WeatherRecent from '../../services/weather/weather-recent';
import { format, formatDuration, intervalToDuration } from 'date-fns';

function Almanac() {
    const [loaded, setLoaded] = useState<boolean>(false);
    const [sunTimes, setSunTimes] = useState<SunCalc.GetTimesResult | null>(null);
    const [moonTimes, setMoonTimes] = useState<SunCalc.GetMoonTimes | null>(null);
    const [moonIllumination, setMoonIllumination] = useState<SunCalc.GetMoonIlluminationResult | null>(null);

    const weatherService = new WeatherService();

    const dayLength = (): string => {
        const duration = intervalToDuration({
            start: sunTimes!.sunrise,
            end: sunTimes!.sunset,
        });

        return formatDuration(duration, { format: ['hours', 'minutes'] });
    };

    const moonPhaseName = (): string => {
        const phase = moonIllumination!.phase;

        if (phase === 0) {
            return 'New Moon';
        } else if (phase < 0.25) {
            return 'Waxing Crescent';
        } else if (phase === 0.25) {
            return 'First Quarter';
        } else if (phase < 0.5) {
            return 'Waxing Gibbous';
        } else if (phase === 0.5) {
            return 'Full Moon';
        } else if (phase < 0.75) {
            return 'Waning Gibbous';
        } else if (phase === 0.75) {
            return 'Last Quarter';
        } else if (phase < 1.0) {
            return 'Waning Crescent';
        }

        return '';
    };

    const moonPhaseLetter = (): string => {
        const phase = moonIllumination!.phase;

        if (phase === 0) {
            return '0';
        } else if (phase < 0.25) {
            return 'D';
        } else if (phase === 0.25) {
            return 'G';
        } else if (phase < 0.5) {
            return 'I';
        } else if (phase === 0.5) {
            return '1';
        } else if (phase < 0.75) {
            return 'Q';
        } else if (phase === 0.75) {
            return 'T';
        } else if (phase < 1.0) {
            return 'W';
        }

        return '';
    };

    useEffect(() => {
        weatherService.getLatest().then((weatherRecent: WeatherRecent) => {
            const date = new Date();

            setSunTimes(SunCalc.getTimes(date, weatherRecent?.latitude!, weatherRecent?.longitude!));
            setMoonTimes(SunCalc.getMoonTimes(date, weatherRecent?.latitude!, weatherRecent?.longitude!));
            setMoonIllumination(SunCalc.getMoonIllumination(date));

            setLoaded(true);
        });
    }, []);

    return (
        <DashboardItem title="Almanac">
            <div className="weather-current">
                {!loaded && <div>Loading...</div>}

                {loaded && (
                    <table>
                        <tbody>
                            <tr>
                                <td className="almanac-table-header">Sunrise</td>
                                <td colSpan={2}>{format(sunTimes!.sunrise, 'hh:mm:ss aa')}</td>
                            </tr>
                            <tr>
                                <td className="almanac-table-header">Sunset</td>
                                <td colSpan={2}>{format(sunTimes!.sunset, 'hh:mm:ss aa')}</td>
                            </tr>
                            <tr>
                                <td className="almanac-table-header">Day length</td>
                                <td colSpan={2}>{dayLength()}</td>
                            </tr>
                            <tr>
                                <td className="almanac-table-header">Moonrise</td>
                                <td colSpan={2}>{format(moonTimes!.rise, 'hh:mm:ss aa')}</td>
                            </tr>
                            <tr>
                                <td className="almanac-table-header">Moonset</td>
                                <td colSpan={2}>{format(moonTimes!.set, 'hh:mm:ss aa')}</td>
                            </tr>
                            <tr>
                                <td className="almanac-table-header">Moon</td>
                                <td>
                                    {moonPhaseName()}
                                    <br />
                                    {(moonIllumination!.fraction * 100).toFixed(1)}% illuminated
                                </td>
                                <td>
                                    <div className="moon-phase">{moonPhaseLetter()}</div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                )}
            </div>
        </DashboardItem>
    );
}

export default Almanac;
