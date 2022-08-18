import { Component, OnInit } from '@angular/core';
import { WeatherUpdate } from 'src/app/models/weather/weather-update';
import { WeatherService } from 'src/app/services/weather/weather.service';
import { first } from 'rxjs/operators';

import * as SunCalc from 'suncalc';
import * as moment from 'moment';
import 'moment-duration-format';

@Component({
    selector: 'app-almanac',
    templateUrl: './almanac.component.html',
    styleUrls: ['./almanac.component.scss']
})
export class AlmanacComponent implements OnInit {
    public loaded = false;
    public latestReading: WeatherUpdate | undefined | null;
    public sunTimes: SunCalc.GetTimesResult | undefined | null;
    public moonTimes: SunCalc.GetMoonTimes | undefined | null;
    public moon: SunCalc.GetMoonIlluminationResult | undefined | null;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.update();

        setInterval(() => this.update(), 60000);
    }

    async update() {
        this.weatherService.getLatestReading().pipe(first(r => r !== null)).subscribe(r => {
            this.latestReading = r;

            const date = new Date();

            this.sunTimes = SunCalc.getTimes(date, this.latestReading?.Latitude!, this.latestReading?.Longitude!);
            this.moonTimes = SunCalc.getMoonTimes(date, this.latestReading?.Latitude!, this.latestReading?.Longitude!);
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

        return '';
    }

    moonPhaseLetter(phase: number): string {
        if (phase === 0) {
            return '0';
        } else if (phase < 0.25) {
            return 'D';
        } else if (phase === 0.25) {
            return 'G';
        } else if (phase < 0.5) {
            return 'I';
        } else if (phase === 0.5) {
            return '1';
        } else if (phase < 0.75) {
            return 'Q';
        } else if (phase === 0.75) {
            return 'T';
        } else if (phase < 1.0) {
            return 'W';
        }

        return '';
    }

    dayLength(): string {
        const duration = moment.duration((this.sunTimes!.sunset.valueOf() - this.sunTimes!.sunrise.valueOf()));
        return duration.format('hh [hours] mm [minutes]');
    }
}
