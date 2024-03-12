import WindDirectionNumber from './models/weather/wind-direction-number';

export function ConvertWindDirectionToDegrees(windDirection: WindDirectionNumber): number {
    switch (windDirection) {
        case WindDirectionNumber.None:
            return -1;
        case WindDirectionNumber.North:
            return 0;
        case WindDirectionNumber.East:
            return 90;
        case WindDirectionNumber.South:
            return 180;
        case WindDirectionNumber.West:
            return 270;
        case WindDirectionNumber.NorthEast:
            return 45;
        case WindDirectionNumber.SouthEast:
            return 135;
        case WindDirectionNumber.SouthWest:
            return 225;
        case WindDirectionNumber.NorthWest:
            return 315;
        case WindDirectionNumber.NorthNorthEast:
            return 22.5;
        case WindDirectionNumber.EastNorthEast:
            return 67.5;
        case WindDirectionNumber.EastSouthEast:
            return 112.5;
        case WindDirectionNumber.SouthSouthEast:
            return 157.5;
        case WindDirectionNumber.SouthSouthWest:
            return 202.5;
        case WindDirectionNumber.WestSouthWest:
            return 247.5;
        case WindDirectionNumber.WestNorthWest:
            return 292.5;
        case WindDirectionNumber.NorthNorthWest:
            return 337.5;
    }
    return -1;
}

export function ConvertDegreesToShortLabel(degrees: number): string {
    switch (degrees) {
        case 0:
        case 360:
            return 'N';
        case 90:
            return 'E';
        case 180:
            return 'S';
        case 270:
            return 'W';
        case 45:
            return 'NE';
        case 135:
            return 'SE';
        case 225:
            return 'SW';
        case 315:
            return 'NW';
        case 22.5:
            return 'NNE';
        case 67.5:
            return 'ENE';
        case 112.5:
            return 'ESE';
        case 157.5:
            return 'SSE';
        case 202.5:
            return 'SSW';
        case 247.5:
            return 'WSW';
        case 292.5:
            return 'WNW';
        case 337.5:
            return 'NNW';
    }

    return '';
}
