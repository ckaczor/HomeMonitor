import { Component, OnInit } from '@angular/core';
import { WeatherReading } from 'src/app/weather-reading';
import { WeatherService } from 'src/app/weather.service';

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
}
