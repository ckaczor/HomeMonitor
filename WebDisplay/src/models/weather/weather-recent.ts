import WindDirection from './wind-direction';

export default class WeatherRecent {
    type: string | undefined;
    message: null | undefined;
    timestamp: Date | undefined;
    windDirection: WindDirection | undefined;
    windSpeed: number | undefined;
    humidity: number | undefined;
    rain: number | undefined;
    pressure: number | undefined;
    temperature: number | undefined;
    batteryLevel: number | undefined;
    lightLevel: number | undefined;
    latitude: number | undefined;
    longitude: number | undefined;
    altitude: number | undefined;
    satelliteCount: number | undefined;
    gpsTimestamp: Date | undefined;
    windChill: number | undefined;
    heatIndex: number | undefined;
    dewPoint: number | undefined;
    pressureDifferenceThreeHour: number | undefined;
    pressureSlope: number | undefined;
    pressureAngle: number | undefined;
    rainLastHour: number | undefined;
}
