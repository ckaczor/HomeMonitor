import WindDirection from './models/weather/wind-direction';

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
