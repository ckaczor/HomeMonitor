import './main.scss';

import { useEffect, useState } from 'react';
import DashboardItem from '../../dashboard-item/main';
import WeatherService from '../../../services/weather/main';
import WeatherUpdate from '../../../services/weather/weather-update';

function CurrentWeather() {
    const [latestReading, setLatestReading] = useState<WeatherUpdate | null>(null);

    const weatherService = new WeatherService();

    useEffect(() => {
        weatherService.start((weatherUpdate: WeatherUpdate) => {
            setLatestReading(weatherUpdate);
        });
    }, []);

    const rotationClass = (pressureDifference: number | undefined) => {
        if (!pressureDifference) {
            return '';
        } else if (Math.abs(pressureDifference) <= 1.0) {
            return '';
        } else if (pressureDifference > 1.0 && pressureDifference <= 2.0) {
            return 'up-low';
        } else if (pressureDifference > 2.0) {
            return 'up-high';
        } else if (pressureDifference < -1.0 && pressureDifference >= -2.0) {
            return 'down-low';
        } else if (pressureDifference < -2.0) {
            return 'down-high';
        }

        return '';
    };

    return (
        <DashboardItem title="Weather">
            <div className="weather-current">
                {latestReading === null && <div>Loading...</div>}
                {latestReading !== null && (
                    <table>
                        <tbody>
                            <tr>
                                <td className="weather-current-header">Temperature</td>
                                <td>
                                    {latestReading!.Temperature?.toFixed(2)}
                                    °F
                                </td>
                            </tr>
                            {latestReading!.HeatIndex && (
                                <tr>
                                    <td className="weather-current-header">Heat index</td>
                                    <td>
                                        {latestReading!.HeatIndex?.toFixed(2)}
                                        °F
                                    </td>
                                </tr>
                            )}
                            {latestReading!.WindChill && (
                                <tr>
                                    <td className="weather-current-header">Wind chill</td>
                                    <td>{latestReading!.WindChill?.toFixed(2)}°F</td>
                                </tr>
                            )}
                            <tr>
                                <td className="weather-current-header">Humidity</td>
                                <td>{latestReading!.Humidity?.toFixed(2)}%</td>
                            </tr>
                            <tr>
                                <td className="weather-current-header">Dew point</td>
                                <td>
                                    {latestReading!.DewPoint?.toFixed(2)}
                                    °F
                                </td>
                            </tr>
                            <tr>
                                <td className="weather-current-header">Pressure</td>
                                <td>
                                    {latestReading!.Pressure && (latestReading!.Pressure / 33.864 / 100)?.toFixed(2)}"
                                    <span
                                        className={'pressure-trend-arrow ' + rotationClass(latestReading!.PressureDifferenceThreeHour)}
                                        title={'3 Hour Change: ' + latestReading!.PressureDifferenceThreeHour?.toFixed(1)}>
                                        ➜
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td className="weather-current-header">Wind</td>
                                <td>
                                    {latestReading!.WindSpeed?.toFixed(2)} mph {latestReading!.WindDirection}
                                </td>
                            </tr>
                            <tr>
                                <td className="weather-current-header">Rain</td>
                                <td>{latestReading!.RainLastHour?.toFixed(2)}" (last hour)</td>
                            </tr>
                            <tr>
                                <td className="weather-current-header">Light</td>
                                <td>{latestReading!.LightLevel?.toFixed(2)} lx</td>
                            </tr>
                        </tbody>
                    </table>
                )}
            </div>
        </DashboardItem>
    );
}

export default CurrentWeather;
