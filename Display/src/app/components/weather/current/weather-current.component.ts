import { Component, OnInit } from '@angular/core';
import { WeatherReading } from 'src/app/models/weather/weather-reading';
import { WeatherService } from 'src/app/services/weather/weather.service';

@Component({
    selector: 'app-weather-current',
    templateUrl: './weather-current.component.html',
    styleUrls: ['./weather-current.component.scss']
})
export class WeatherCurrentComponent implements OnInit {
    public latestReading: WeatherReading;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.weatherService.getLatestReading().subscribe(r => this.latestReading = r);
    }

    rotationClass(): string {
        const pressureDifference = this.latestReading.PressureTrend;

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
    }
}
