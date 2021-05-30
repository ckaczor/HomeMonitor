export interface WeatherAggregates {
    humidity: WeatherAggregate;
    temperature: WeatherAggregate;
    pressure: WeatherAggregate;
    light: WeatherAggregate;
    windSpeed: WeatherAggregate;
    windDirectionAverage: number;
    rainTotal: number;
}

export interface WeatherAggregate {
    min: number;
    max: number;
    average: number;
}
