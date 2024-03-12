import WindDirection from './models/weather/wind-direction';
import WindDirectionNumber from './models/weather/wind-direction-number';

export function ShortenWindDirection(windDirection: WindDirection | undefined): string {
    switch (windDirection) {
        case WindDirection.None:
            return '';
        case WindDirection.North:
            return 'N';
        case WindDirection.East:
            return 'E';
        case WindDirection.South:
            return 'S';
        case WindDirection.West:
            return 'W';
        case WindDirection.NorthEast:
            return 'NE';
        case WindDirection.SouthEast:
            return 'SE';
        case WindDirection.SouthWest:
            return 'SW';
        case WindDirection.NorthWest:
            return 'NW';
        case WindDirection.NorthNorthEast:
            return 'NNE';
        case WindDirection.EastNorthEast:
            return 'ENE';
        case WindDirection.EastSouthEast:
            return 'ESE';
        case WindDirection.SouthSouthEast:
            return 'SSE';
        case WindDirection.SouthSouthWest:
            return 'SSW';
        case WindDirection.WestSouthWest:
            return 'WSW';
        case WindDirection.WestNorthWest:
            return 'WNW';
        case WindDirection.NorthNorthWest:
            return 'NNW';
    }
    return windDirection!.toString();
}

export function ShortenWindDirectionNumber(windDirection: WindDirectionNumber | undefined): string {
    switch (windDirection) {
        case WindDirectionNumber.None:
            return '';
        case WindDirectionNumber.North:
            return 'N';
        case WindDirectionNumber.East:
            return 'E';
        case WindDirectionNumber.South:
            return 'S';
        case WindDirectionNumber.West:
            return 'W';
        case WindDirectionNumber.NorthEast:
            return 'NE';
        case WindDirectionNumber.SouthEast:
            return 'SE';
        case WindDirectionNumber.SouthWest:
            return 'SW';
        case WindDirectionNumber.NorthWest:
            return 'NW';
        case WindDirectionNumber.NorthNorthEast:
            return 'NNE';
        case WindDirectionNumber.EastNorthEast:
            return 'ENE';
        case WindDirectionNumber.EastSouthEast:
            return 'ESE';
        case WindDirectionNumber.SouthSouthEast:
            return 'SSE';
        case WindDirectionNumber.SouthSouthWest:
            return 'SSW';
        case WindDirectionNumber.WestSouthWest:
            return 'WSW';
        case WindDirectionNumber.WestNorthWest:
            return 'WNW';
        case WindDirectionNumber.NorthNorthWest:
            return 'NNW';
    }
    return windDirection!.toString();
}
