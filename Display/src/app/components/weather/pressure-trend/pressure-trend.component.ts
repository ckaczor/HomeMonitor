import { Component, OnInit } from '@angular/core';
import { WeatherService } from 'src/app/services/weather/weather.service';
import { WeatherValueType } from 'src/app/models/weather/weather-value-type';
import { WeatherValue } from 'src/app/models/weather/weather-value';

import * as moment from 'moment';
import * as regression from 'regression';

@Component({
    selector: 'app-pressure-trend',
    templateUrl: './pressure-trend.component.html',
    styleUrls: ['./pressure-trend.component.scss']
})
export class PressureTrendComponent implements OnInit {
    public pressureDifference: number = null;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.update();

        setInterval(this.update, 60000);
    }

    async update() {
        const end: moment.Moment = moment();
        const start: moment.Moment = moment(end).subtract(3, 'hours');

        const weatherData = await this.weatherService.getReadingValueHistory(WeatherValueType.Pressure, start, end);

        if (!weatherData) {
            return;
        }

        const points: Array<Array<number>> = [];

        weatherData.forEach((weatherValue: WeatherValue) => {
            const point = [moment(weatherValue.timestamp).unix(), weatherValue.value / 100];
            points.push(point);
        });

        const result = regression.linear(points, { precision: 10 });

        const regressionPoints = result.points;

        this.pressureDifference = regressionPoints[regressionPoints.length - 1][1] - regressionPoints[0][1];
    }

    rotationClass(): string {
        if (!this.pressureDifference) {
            return '';
        } else if (Math.abs(this.pressureDifference) <= 1.0) {
            return '';
        } else if (this.pressureDifference > 1.0 && this.pressureDifference <= 2.0) {
            return 'up-low';
        } else if (this.pressureDifference > 2.0) {
            return 'up-high';
        } else if (this.pressureDifference < -1.0 && this.pressureDifference >= -2.0) {
            return 'down-low';
        } else if (this.pressureDifference < -2.0) {
            return 'down-high';
        }
    }
}
