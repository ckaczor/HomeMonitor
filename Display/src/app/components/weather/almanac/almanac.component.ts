import { Component, OnInit } from '@angular/core';
import { WeatherReading } from 'src/app/models/weather/weather-reading';
import { WeatherService } from 'src/app/services/weather/weather.service';
import { first } from 'rxjs/operators';

import * as SunCalc from 'suncalc';

@Component({
    selector: 'app-almanac',
    templateUrl: './almanac.component.html',
    styleUrls: ['./almanac.component.scss']
})
export class AlmanacComponent implements OnInit {
    public loaded = false;
    public latestReading: WeatherReading;
    public sun: SunCalc.GetTimesResult;
    public moon: SunCalc.GetMoonIlluminationResult;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.update();

        setInterval(() => this.update(), 60000);
    }

    async update() {
        this.weatherService.getLatestReading().pipe(first(r => r !== null)).subscribe(r => {
            this.latestReading = r;

            const date = new Date();

            this.sun = SunCalc.getTimes(date, this.latestReading.Latitude, this.latestReading.Longitude);
            this.moon = SunCalc.getMoonIllumination(date);

            this.loaded = true;
        });
    }

    moonPhaseName(phase: number): string {
        if (phase === 0) {
            return 'New Moon';
        } else if (phase < 0.25) {
            return 'Waxing Crescent';
        } else if (phase === 0.25) {
            return 'First Quarter';
        } else if (phase < 0.5) {
            return 'Waxing Gibbous';
        } else if (phase === 0.5) {
            return 'Full Moon';
        } else if (phase < 0.75) {
            return 'Waning Gibbous';
        } else if (phase === 0.75) {
            return 'Last Quarter';
        } else if (phase < 1.0) {
            return 'Waning Crescent';
        }
    }
}
