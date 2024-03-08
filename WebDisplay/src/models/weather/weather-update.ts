import WindDirection from './wind-direction';

export default class WeatherUpdate {
    Type: string | undefined;
    Message: null | undefined;
    Timestamp: Date | undefined;
    WindDirection: WindDirection | undefined;
    WindSpeed: number | undefined;
    Humidity: number | undefined;
    Rain: number | undefined;
    Pressure: number | undefined;
    Temperature: number | undefined;
    BatteryLevel: number | undefined;
    LightLevel: number | undefined;
    Latitude: number | undefined;
    Longitude: number | undefined;
    Altitude: number | undefined;
    SatelliteCount: number | undefined;
    GpsTimestamp: Date | undefined;
    WindChill: number | undefined;
    HeatIndex: number | undefined;
    DewPoint: number | undefined;
    PressureDifferenceThreeHour: number | undefined;
    PressureSlope: number | undefined;
    PressureAngle: number | undefined;
    RainLastHour: number | undefined;
}
