import WindDirection from './wind-direction';

export default interface WeatherAggregates {
    humidity: WeatherAggregate;
    temperature: WeatherAggregate;
    pressure: WeatherAggregate;
    light: WeatherAggregate;
    windSpeed: WeatherAggregate;
    windDirectionAverage: WindDirection;
    rainTotal: number;
}

export interface WeatherAggregate {
    min: number;
    max: number;
    average: number;
}
